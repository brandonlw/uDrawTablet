using System;
using System.Collections.Generic;
using System.Text;

namespace uDrawTablet
{
  public static class Keypress
  {
    public const KeyCode CTRL_MASK = (KeyCode)0x0100;
    public const KeyCode SHIFT_MASK = (KeyCode)0x0200;
    public const KeyCode ALT_MASK = (KeyCode)0x0400;
    public const KeyCode WIN_MASK = (KeyCode)0x0800;
    public const KeyCode SEND_ONCE_MASK = (KeyCode)0x1000;

    public class ChatpadKeyCode
    {
      public byte Key { get; set; }
      public bool OrangeHeld { get; set; }
      public bool GreenHeld { get; set; }
      public bool ShiftHeld { get; set; }

      public ChatpadKeyCode(byte key)
      {
        Key = key;
      }

      public ChatpadKeyCode(byte key, bool orange, bool green, bool shift)
      {
        Key = key;
        OrangeHeld = orange;
        GreenHeld = green;
        ShiftHeld = shift;
      }

      public override int GetHashCode()
      {
        return this.ToString().GetHashCode();
      }

      public override bool Equals(object obj)
      {
        var o = obj as ChatpadKeyCode;

        if (obj == null)
          return base.Equals(obj);
        else
          return (this.Key == o.Key && this.OrangeHeld == o.OrangeHeld &&
            this.GreenHeld == o.GreenHeld && this.ShiftHeld == o.ShiftHeld);
      }

      public override string ToString()
      {
        return this.Key.ToString() + this.OrangeHeld + this.GreenHeld + this.ShiftHeld;
      }
    };

    public static Dictionary<ChatpadKeyCode, KeyCode> ChatpadKeyCodes { get; set; }

    static Keypress()
    {
      ChatpadKeyCodes = new Dictionary<ChatpadKeyCode, KeyCode>();
      ChatpadKeyCodes.Add(new ChatpadKeyCode(23), GetFullKeyCode(KeyCode.NumberPad1));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(22), GetFullKeyCode(KeyCode.NumberPad2));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(21), GetFullKeyCode(KeyCode.NumberPad3));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(20), GetFullKeyCode(KeyCode.NumberPad4));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(19), GetFullKeyCode(KeyCode.NumberPad5));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(18), GetFullKeyCode(KeyCode.NumberPad6));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(17), GetFullKeyCode(KeyCode.NumberPad7));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(103), GetFullKeyCode(KeyCode.NumberPad8));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(102), GetFullKeyCode(KeyCode.NumberPad9));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(101), GetFullKeyCode(KeyCode.NumberPad0));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(39), GetFullKeyCode(KeyCode.Q));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(39, false, true, false), GetFullKeyCode(KeyCode.One, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(39, false, false, true), GetFullKeyCode(KeyCode.Q, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(38), GetFullKeyCode(KeyCode.W));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(38, false, true, false), GetFullKeyCode(KeyCode.Two, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(38, false, false, true), GetFullKeyCode(KeyCode.W, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(37), GetFullKeyCode(KeyCode.E));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(37, false, false, true), GetFullKeyCode(KeyCode.E, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(36), GetFullKeyCode(KeyCode.R));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(36, false, true, false), GetFullKeyCode(KeyCode.Three, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(36, false, false, true), GetFullKeyCode(KeyCode.R, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(36, true, false, false), GetFullKeyCode(KeyCode.Four, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(35), GetFullKeyCode(KeyCode.T));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(35, false, true, false), GetFullKeyCode(KeyCode.Five, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(35, false, false, true), GetFullKeyCode(KeyCode.T, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(34), GetFullKeyCode(KeyCode.Y));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(34, false, true, false), GetFullKeyCode(KeyCode.Six, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(34, false, false, true), GetFullKeyCode(KeyCode.Y, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(33), GetFullKeyCode(KeyCode.U));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(33, false, true, false), GetFullKeyCode(KeyCode.Seven, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(33, false, false, true), GetFullKeyCode(KeyCode.U, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(118), GetFullKeyCode(KeyCode.I));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(118, false, true, false), GetFullKeyCode(KeyCode.Eight, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(118, false, false, true), GetFullKeyCode(KeyCode.I, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(117), GetFullKeyCode(KeyCode.O));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(117, false, true, false), GetFullKeyCode(KeyCode.Nine, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(117, false, false, true), GetFullKeyCode(KeyCode.O, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(100), GetFullKeyCode(KeyCode.P));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(100, false, true, false), GetFullKeyCode(KeyCode.Zero, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(100, false, false, true), GetFullKeyCode(KeyCode.P, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(100, true, false, false), GetFullKeyCode(KeyCode.Plus, false, false, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(55), GetFullKeyCode(KeyCode.A));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(55, false, true, false), GetFullKeyCode(KeyCode.Tilde, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(55, false, false, true), GetFullKeyCode(KeyCode.A, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(54), GetFullKeyCode(KeyCode.S));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(54, false, false, true), GetFullKeyCode(KeyCode.S, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(53), GetFullKeyCode(KeyCode.D));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(53, false, true, false), GetFullKeyCode(KeyCode.LeftBracket, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(53, false, false, true), GetFullKeyCode(KeyCode.D, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(52), GetFullKeyCode(KeyCode.F));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(52, false, true, false), GetFullKeyCode(KeyCode.RightBracket, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(52, false, false, true), GetFullKeyCode(KeyCode.F, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(51), GetFullKeyCode(KeyCode.G));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(51, false, false, true), GetFullKeyCode(KeyCode.G, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(50), GetFullKeyCode(KeyCode.H));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(50, false, true, false), GetFullKeyCode(KeyCode.ForwardSlash, false, false, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(50, false, false, true), GetFullKeyCode(KeyCode.H, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(50, true, false, false), GetFullKeyCode(KeyCode.Backslash, false, false, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(49), GetFullKeyCode(KeyCode.J));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(49, false, true, false), GetFullKeyCode(KeyCode.Quote, false, false, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(49, false, false, true), GetFullKeyCode(KeyCode.J, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(49, true, false, false), GetFullKeyCode(KeyCode.Quote, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(119), GetFullKeyCode(KeyCode.K));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(119, false, true, false), GetFullKeyCode(KeyCode.LeftBracket, false, false, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(119, false, false, true), GetFullKeyCode(KeyCode.K, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(114), GetFullKeyCode(KeyCode.L));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(114, false, true, false), GetFullKeyCode(KeyCode.RightBracket, false, false, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(114, false, false, true), GetFullKeyCode(KeyCode.L, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(98), GetFullKeyCode(KeyCode.Comma));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(98, false, true, false), GetFullKeyCode(KeyCode.Semicolon, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(98, true, false, false), GetFullKeyCode(KeyCode.Semicolon, false, false, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(70), GetFullKeyCode(KeyCode.Z));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(70, false, true, false), GetFullKeyCode(KeyCode.Tilde, false, false, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(70, false, false, true), GetFullKeyCode(KeyCode.Z, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(69), GetFullKeyCode(KeyCode.X));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(69, false, false, true), GetFullKeyCode(KeyCode.X, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(68), GetFullKeyCode(KeyCode.C));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(68, false, false, true), GetFullKeyCode(KeyCode.C, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(67), GetFullKeyCode(KeyCode.V));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(67, false, true, false), GetFullKeyCode(KeyCode.Minus, false, false, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(67, false, false, true), GetFullKeyCode(KeyCode.V, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(67, true, false, false), GetFullKeyCode(KeyCode.Minus, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(66), GetFullKeyCode(KeyCode.B));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(66, false, true, false), GetFullKeyCode(KeyCode.Backslash, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(66, false, false, true), GetFullKeyCode(KeyCode.B, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(66, true, false, false), GetFullKeyCode(KeyCode.Plus, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(65), GetFullKeyCode(KeyCode.N));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(65, false, true, false), GetFullKeyCode(KeyCode.Comma, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(65, false, false, true), GetFullKeyCode(KeyCode.N, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(82), GetFullKeyCode(KeyCode.M));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(82, false, true, false), GetFullKeyCode(KeyCode.Period, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(82, false, false, true), GetFullKeyCode(KeyCode.M, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(83), GetFullKeyCode(KeyCode.Period));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(83, false, true, false), GetFullKeyCode(KeyCode.ForwardSlash, false, true, false, false));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(99), GetFullKeyCode(KeyCode.Enter));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(85), GetFullKeyCode(KeyCode.Left));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(84), GetFullKeyCode(KeyCode.Space));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(81), GetFullKeyCode(KeyCode.Right));
      ChatpadKeyCodes.Add(new ChatpadKeyCode(113), GetFullKeyCode(KeyCode.Backspace));
    }

    public static KeyCode? GetChatpadKeyCode(ChatpadKeyCode key)
    {
      KeyCode? ret = null;

      if (ChatpadKeyCodes.ContainsKey(key))
        ret = ChatpadKeyCodes[key];

      return ret;
    }

    public static KeyCode GetFullKeyCode(KeyCode key)
    {
      return GetFullKeyCode(key, false, false, false, false);
    }

    public static KeyCode GetFullKeyCode(KeyCode key, bool ctrl, bool shift, bool alt, bool win)
    {
      return GetFullKeyCode(key, ctrl, shift, alt, win, true);
    }

    public static KeyCode GetFullKeyCode(KeyCode key, bool ctrl, bool shift, bool alt, bool win, bool sendOnce)
    {
      KeyCode ret = key;

      if (ctrl) ret |= CTRL_MASK;
      if (shift) ret |= SHIFT_MASK;
      if (alt) ret |= ALT_MASK;
      if (win) ret |= WIN_MASK;
      if (sendOnce) ret |= SEND_ONCE_MASK;

      return ret;
    }

    public enum ModifierKeyCode
    {
      Shift = 0x10,
      Control = 0x11,
      Alt = 0x12,
      Windows = 0x5B
    };

    public enum KeyCode
    {
      None = 0x00,
      Backspace = 0x08,
      Tab = 0x09,
      Enter = 0x0D,
      Pause = 0x13,
      CapsLock = 0x14,
      Escape = 0x1B,
      Space = 0x20,
      PageUp = 0x21,
      PageDown = 0x22,
      End = 0x23,
      Home = 0x24,
      Left = 0x25,
      Up = 0x26,
      Right = 0x27,
      Down = 0x28,
      PrintScreen = 0x2C,
      Insert = 0x2D,
      Delete = 0x2E,
      Zero = 0x30,
      One = 0x31,
      Two = 0x32,
      Three = 0x33,
      Four = 0x34,
      Five = 0x35,
      Six = 0x36,
      Seven = 0x37,
      Eight = 0x38,
      Nine = 0x39,
      A = 0x41,
      B = 0x42,
      C = 0x43,
      D = 0x44,
      E = 0x45,
      F = 0x46,
      G = 0x47,
      H = 0x48,
      I = 0x49,
      J = 0x4A,
      K = 0x4B,
      L = 0x4C,
      M = 0x4D,
      N = 0x4E,
      O = 0x4F,
      P = 0x50,
      Q = 0x51,
      R = 0x52,
      S = 0x53,
      T = 0x54,
      U = 0x55,
      V = 0x56,
      W = 0x57,
      X = 0x58,
      Y = 0x59,
      Z = 0x5A,
      NumberPad0 = 0x60,
      NumberPad1 = 0x61,
      NumberPad2 = 0x62,
      NumberPad3 = 0x63,
      NumberPad4 = 0x64,
      NumberPad5 = 0x65,
      NumberPad6 = 0x66,
      NumberPad7 = 0x67,
      NumberPad8 = 0x68,
      NumberPad9 = 0x69,
      Multiply = 0x6A,
      Add = 0x6B,
      Separator = 0x6C,
      Subtract = 0x6D,
      Decimal = 0x6E,
      Divide = 0x6F,
      F1 = 0x70,
      F2 = 0x71,
      F3 = 0x72,
      F4 = 0x73,
      F5 = 0x74,
      F6 = 0x75,
      F7 = 0x76,
      F8 = 0x77,
      F9 = 0x78,
      F10 = 0x79,
      F11 = 0x7A,
      F12 = 0x7B,
      Back = 0xA6,
      Forward = 0xA7,
      Semicolon = 0xBA,
      Plus = 0xBB,
      Comma = 0xBC,
      Minus = 0xBD,
      Period = 0xBE,
      ForwardSlash = 0xBF,
      Tilde = 0xC0,
      LeftBracket = 0xDB,
      Backslash = 0xDC,
      RightBracket = 0xDD,
      Quote = 0xDE
    };
  }
}
