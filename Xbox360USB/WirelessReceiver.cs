using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using LibUsbDotNet;
using LibUsbDotNet.DeviceNotify;
using LibUsbDotNet.Main;

namespace Xbox360USB
{
  public class WirelessReceiver : IDisposable
  {
    #region Declarations

    private const int _VENDOR_ID = 0x045E;
    private const int _PRODUCT_ID1 = 0x028E;
    private const int _PRODUCT_ID2 = 0x0291;
    private const int _NUMBER_OF_SLOTS = 4;
    private const int _WRITE_TIMEOUT = 5000;
    private const int _WAIT_MS = 10;
    private IUsbDevice _device;
    private Dictionary<int, ReceiverSlot> _slots;
    private Dictionary<int, List<byte[]>> _responses;
    private Thread _refreshSlotsThread;

    public event EventHandler<DeviceEventArgs> DeviceConnected;
    public event EventHandler<DeviceEventArgs> DeviceDisconnected;
    public event EventHandler<DeviceEventArgs> HeadsetConnected;
    public event EventHandler<DeviceEventArgs> HeadsetDisconnected;
    public event EventHandler<DataReceivedEventArgs> HeadsetDataReceived;
    public event EventHandler<DataReceivedEventArgs> EventDataReceived;
    public event EventHandler<DataReceivedEventArgs> UnknownDataReceived;

    private class ReceiverSlot
    {
      public bool RefreshToggle { get; set; }
      public bool? IsDeviceConnected { get; set; }
      public bool? IsHeadsetConnected { get; set; }
      public DeviceInformation DeviceInformation { get; set; }
      public UsbEndpointReader DataReader { get; set; }
      public UsbEndpointWriter DataWriter { get; set; }
      public UsbEndpointReader HeadsetReader { get; set; }
      public UsbEndpointWriter HeadsetWriter { get; set; }
    };

    public enum DeviceSubtype
    {
      Controller = 0x01,
      uDrawTablet = 0x23
    };

    public class DeviceInformation
    {
      public byte[] Serial { get; internal set; }
      public DeviceSubtype Subtype { get; internal set; }

      public DeviceInformation(byte[] serial, int subType)
      {
        Serial = serial;
        Subtype = (DeviceSubtype)subType;
      }

      public override string ToString()
      {
        return BitConverter.ToString(Serial).Replace("-", String.Empty).ToUpper();
      }
    };

    #endregion

    #region Constructors/Teardown

    public WirelessReceiver()
    {
      //Do nothing
    }

    #endregion

    #region Public Methods

    public void Start()
    {
      Start(false);
    }

    public void Start(bool throwErrors)
    {
      _Init(throwErrors);
    }

    public bool IsDeviceConnected(int index)
    {
      bool ret = false;

      if (_slots != null && _slots.ContainsKey(index))
        ret = _slots[index].IsDeviceConnected.GetValueOrDefault();

      return ret;
    }

    public bool IsHeadsetConnected(int index)
    {
      bool ret = false;

      if (_slots != null && _slots.ContainsKey(index))
        ret = _slots[index].IsHeadsetConnected.GetValueOrDefault();

      return ret;
    }

    public DeviceInformation GetDeviceInformation(int index)
    {
      DeviceInformation ret = null;

      if (_slots != null && _slots.ContainsKey(index))
      {
        if (_slots[index].IsDeviceConnected.GetValueOrDefault())
        {
          if (_slots[index].DeviceInformation == null)
            _RefreshSlot(index);
        }

        ret = _slots[index].DeviceInformation;
      }

      return ret;
    }

    public void WriteHeadsetData(int index, byte[] data)
    {
      int transferred;

      if (_slots != null && _slots.ContainsKey(index))
        _slots[index].HeadsetWriter.Write(data, _WRITE_TIMEOUT, out transferred);
    }

    public void WriteData(int index, byte[] data)
    {
      int transferred;

      if (_slots != null && _slots.ContainsKey(index))
        _slots[index].DataWriter.Write(data, _WRITE_TIMEOUT, out transferred);
    }

    public void SetLEDStatus(int index, byte status)
    {
      var data = new byte[12];
      data[2] = 0x08;
      data[3] = (byte)(0x40 + status);
      this.WriteData(index, data);
    }

    public void TurnOffDevice(int index)
    {
      var data = new byte[12];
      data[2] = 0x08;
      data[3] = 0xC0;
      this.WriteData(index, data);
    }

    public void Dispose()
    {
      try
      {
        if (_refreshSlotsThread != null)
        {
          _refreshSlotsThread.Abort();
          _refreshSlotsThread = null;
        }

        if (_slots != null)
        {
          foreach (var slot in _slots.Values)
          {
            if (slot != null)
            {
              slot.DataReader.DataReceivedEnabled = false;
              slot.DataReader.DataReceived -= DataReader_DataReceived;
              slot.DataReader.Abort();
              slot.DataReader.Dispose();
              slot.HeadsetReader.DataReceivedEnabled = false;
              slot.HeadsetReader.DataReceived -= HeadsetReader_DataReceived;
              slot.HeadsetReader.Abort();
              slot.HeadsetReader.Dispose();
            }
          }
        }
        _slots = new Dictionary<int, ReceiverSlot>();

        if (_device != null)
        {
          if (_device.IsOpen)
            _device.Close();
          _device = null;
        }
      }
      catch
      {
        //Don't care...
      }
    }

    #endregion

    #region Public Properties

    public bool IsReceiverConnected
    {
      get
      {
        return (_device != null) && _device.IsOpen;
      }
    }

    #endregion

    #region Local Methods

    private void _Init(bool throwErrors)
    {
      Dispose();

      _device = UsbDevice.OpenUsbDevice(new UsbDeviceFinder(_VENDOR_ID, _PRODUCT_ID1)) as IUsbDevice;
      if (_device == null) _device = UsbDevice.OpenUsbDevice(new UsbDeviceFinder(_VENDOR_ID, _PRODUCT_ID2)) as IUsbDevice;
      if (_device == null)
      {
        if (throwErrors)
          throw new InvalidOperationException("Wireless receiver not found");
      }
      else
      {
        _device.ClaimInterface(1);
        _device.SetConfiguration(1);

        _responses = new Dictionary<int, List<byte[]>>();
        var now = DateTime.Now;
        for (int i = 0; i < _NUMBER_OF_SLOTS; i++)
        {
          var slot = new ReceiverSlot();

          slot.DataWriter = _device.OpenEndpointWriter(_GetWriteEndpoint((i * 2) + 1));
          slot.DataReader = _device.OpenEndpointReader(_GetReadEndpoint((i * 2) + 1));
          slot.DataReader.DataReceived += DataReader_DataReceived;
          slot.DataReader.DataReceivedEnabled = true;
          slot.HeadsetWriter = _device.OpenEndpointWriter(_GetWriteEndpoint((i * 2) + 2));
          slot.HeadsetReader = _device.OpenEndpointReader(_GetReadEndpoint((i * 2) + 2));
          slot.HeadsetReader.DataReceived += HeadsetReader_DataReceived;
          slot.HeadsetReader.DataReceivedEnabled = true;

          _slots.Add(i + 1, slot);

          _RefreshSlot(i + 1);
        }

        _refreshSlotsThread = new Thread(new ThreadStart(_RefreshSlots));
        _refreshSlotsThread.IsBackground = true;
        _refreshSlotsThread.Start();
      }
    }

    private void _RefreshSlot(int index)
    {
      //Refresh the status on this slot (send more than once because it's a little flaky)
      _RefreshConnectionStatus(index);
      _RefreshConnectionStatus(index);

      var start = DateTime.Now;
      while (DateTime.Now < start.AddMilliseconds(_WRITE_TIMEOUT) && (!_slots[index].IsDeviceConnected.HasValue ||
        (_slots[index].IsDeviceConnected.Value && _slots[index].DeviceInformation == null)))
      {
        Thread.Sleep(_WAIT_MS);
      }
    }

    private void _RefreshConnectionStatus(int index)
    {
      var data = new byte[12];
      data[0] = 0x08;
      this.WriteData(index, data);
    }

    private void _RefreshSlots()
    {
      var data = new byte[12];
      data[2] = 0x0C;

      while (true)
      {
        try
        {
          foreach (var slot in _slots)
          {
            if (slot.Value.IsDeviceConnected.GetValueOrDefault())
            {
              data[3] = (byte)(slot.Value.RefreshToggle ? 0x1E : 0x1F);
              slot.Value.RefreshToggle = !slot.Value.RefreshToggle;
              this.WriteData(slot.Key, data);
            }
          }
        }
        catch
        {
          //Whatever...
        }

        Thread.Sleep(1000);
      }
    }

    private WriteEndpointID _GetWriteEndpoint(int endpoint)
    {
      WriteEndpointID ret;

      if (endpoint == 0x01)
        ret = WriteEndpointID.Ep01;
      else if (endpoint == 0x02)
        ret = WriteEndpointID.Ep02;
      else if (endpoint == 0x03)
        ret = WriteEndpointID.Ep03;
      else if (endpoint == 0x04)
        ret = WriteEndpointID.Ep04;
      else if (endpoint == 0x05)
        ret = WriteEndpointID.Ep05;
      else if (endpoint == 0x06)
        ret = WriteEndpointID.Ep06;
      else if (endpoint == 0x07)
        ret = WriteEndpointID.Ep07;
      else if (endpoint == 0x08)
        ret = WriteEndpointID.Ep08;
      else if (endpoint == 0x09)
        ret = WriteEndpointID.Ep09;
      else if (endpoint == 0x0A)
        ret = WriteEndpointID.Ep10;
      else if (endpoint == 0x0B)
        ret = WriteEndpointID.Ep11;
      else if (endpoint == 0x0C)
        ret = WriteEndpointID.Ep12;
      else if (endpoint == 0x0D)
        ret = WriteEndpointID.Ep13;
      else if (endpoint == 0x0E)
        ret = WriteEndpointID.Ep14;
      else if (endpoint == 0x0F)
        ret = WriteEndpointID.Ep15;
      else
        throw new InvalidOperationException("Invalid endpoint ID");

      return ret;
    }

    private ReadEndpointID _GetReadEndpoint(int endpoint)
    {
      ReadEndpointID ret;

      if (endpoint == 0x01)
        ret = ReadEndpointID.Ep01;
      else if (endpoint == 0x02)
        ret = ReadEndpointID.Ep02;
      else if (endpoint == 0x03)
        ret = ReadEndpointID.Ep03;
      else if (endpoint == 0x04)
        ret = ReadEndpointID.Ep04;
      else if (endpoint == 0x05)
        ret = ReadEndpointID.Ep05;
      else if (endpoint == 0x06)
        ret = ReadEndpointID.Ep06;
      else if (endpoint == 0x07)
        ret = ReadEndpointID.Ep07;
      else if (endpoint == 0x08)
        ret = ReadEndpointID.Ep08;
      else if (endpoint == 0x09)
        ret = ReadEndpointID.Ep09;
      else if (endpoint == 0x0A)
        ret = ReadEndpointID.Ep10;
      else if (endpoint == 0x0B)
        ret = ReadEndpointID.Ep11;
      else if (endpoint == 0x0C)
        ret = ReadEndpointID.Ep12;
      else if (endpoint == 0x0D)
        ret = ReadEndpointID.Ep13;
      else if (endpoint == 0x0E)
        ret = ReadEndpointID.Ep14;
      else if (endpoint == 0x0F)
        ret = ReadEndpointID.Ep15;
      else
        throw new InvalidOperationException("Invalid endpoint ID");

      return ret;
    }

    #endregion

    #region Event Handlers

    private void HeadsetReader_DataReceived(object sender, EndpointDataEventArgs e)
    {
      var buffer = new byte[e.Count];
      Array.Copy(e.Buffer, 0, buffer, 0, buffer.Length);
      UsbEndpointReader reader = sender as UsbEndpointReader;

      if (reader != null)
      {
        //Find the slot this belongs to
        int? index = null;
        foreach (var slot in _slots)
        {
          if (slot.Value.DataReader == reader)
          {
            index = slot.Key;
            break;
          }
        }

        if (index.HasValue)
        {
          if (HeadsetDataReceived != null)
            HeadsetDataReceived(this, new DataReceivedEventArgs(index.Value, buffer));
        }
      }
    }

    //private byte[] _oldData = null;
    private void DataReader_DataReceived(object sender, EndpointDataEventArgs e)
    {
      var buffer = new byte[e.Count];
      Array.Copy(e.Buffer, 0, buffer, 0, buffer.Length);
      UsbEndpointReader reader = sender as UsbEndpointReader;

      if (reader != null)
      {
        //Find the slot this belongs to
        int? index = null;
        foreach (var slot in _slots)
        {
          if (slot.Value.DataReader == reader)
          {
            index = slot.Key;
            break;
          }
        }

        if (index.HasValue)
        {
          //Console.WriteLine(String.Format("Index {0}, Data: {1}", index.Value, BitConverter.ToString(buffer)));

          if (buffer[0] == 0x08)
          {
            //This is a status packet
            bool headsetConnected = ((buffer[1] & 0x40) > 0);
            bool deviceConnected = ((buffer[1] & 0x80) > 0);

            bool? wasHeadsetConnected = _slots[index.Value].IsHeadsetConnected;
            _slots[index.Value].IsHeadsetConnected = headsetConnected;
            if (headsetConnected && !wasHeadsetConnected.GetValueOrDefault())
            {
              //Console.WriteLine("Headset connected");

              if (HeadsetConnected != null)
                HeadsetConnected(this, new DeviceEventArgs(index.Value));
            }
            else if (!headsetConnected && wasHeadsetConnected.GetValueOrDefault())
            {
              //Console.WriteLine("Headset disconnected");

              if (HeadsetDisconnected != null)
                HeadsetDisconnected(this, new DeviceEventArgs(index.Value));
            }

            bool? wasDeviceConnected = _slots[index.Value].IsDeviceConnected;
            if (!deviceConnected)
              _slots[index.Value].IsDeviceConnected = false;
            if (!deviceConnected && wasDeviceConnected.GetValueOrDefault())
            {
              //Console.WriteLine("Device disconnected");

              if (DeviceDisconnected != null)
                DeviceDisconnected(this, new DeviceEventArgs(index.Value));
            }

            if (!deviceConnected)
              _slots[index.Value].DeviceInformation = null;
            else
            {
              //Device is connected -- set the LED status appropriately
              this.SetLEDStatus(index.Value, (byte)(0x01 + index.Value));

              //Go ahead and enable chatpad events, just in case
              this.WriteData(index.Value, new byte[] { 0x00, 0x00, 0x0C, 0x1B, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
            }
          }
          else if (buffer[0] == 0x00 && buffer[1] == 0x0F && buffer[2] == 0x00 && buffer[3] == 0xF0)
          {
            byte[] serial = new byte[7];
            Array.Copy(buffer, 0x07, serial, 0, serial.Length);
            int subType = buffer[0x19];
            _slots[index.Value].DeviceInformation = new DeviceInformation(serial, subType);

            if (!_slots[index.Value].IsDeviceConnected.GetValueOrDefault())
            {
              _slots[index.Value].IsDeviceConnected = true;
              //Console.WriteLine("Device connected");

              if (DeviceConnected != null)
                DeviceConnected(this, new DeviceEventArgs(index.Value));
            }
          }
          else if (buffer[0] == 0x00 && buffer[2] == 0x00 && buffer[3] == 0xF0)
          {
            //This is event data

            switch (buffer[1])
            {
              case 0x00:
                {
                  //Unknown event data, comes in after every button press, always zeroes
                  //Eating it for now until we know better
                  break;
                }
              case 0x01:
                {
                  //Regular device data
                  /*
                  if (_oldData == null) _oldData = buffer;
                  bool isSameData = true;
                  if (_oldData.Length == buffer.Length)
                  {
                    for (int i = 0; i < buffer.Length; i++)
                    {
                      if (_oldData[i] != buffer[i])
                      {
                        isSameData = false;
                        break;
                      }
                    }
                  }
                  else
                    isSameData = false;

                  _oldData = buffer;
                  if (!isSameData)
                    Console.WriteLine("Device data: " + BitConverter.ToString(buffer));
                    */

                  if (EventDataReceived != null)
                    EventDataReceived(this, new DataReceivedEventArgs(index.Value, buffer));
                  break;
                }
              case 0x02:
                {
                  //Wireless chatpad data
                  //Console.WriteLine("Chatpad data: " + BitConverter.ToString(buffer));

                  if (EventDataReceived != null)
                    EventDataReceived(this, new DataReceivedEventArgs(index.Value, buffer));
                  break;
                }
              default:
                {
                  //Console.WriteLine("Unknown data: " + BitConverter.ToString(buffer));

                  if (UnknownDataReceived != null)
                    UnknownDataReceived(this, new DataReceivedEventArgs(index.Value, buffer));
                  break;
                }
            }
          }
        }
      }
    }

    #endregion
  }
}
