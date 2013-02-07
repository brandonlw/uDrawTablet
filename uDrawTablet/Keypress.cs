using System;
using System.Collections.Generic;
using System.Text;

namespace uDrawTablet
{
  public class Keypress
  {
    public const KeyCode CTRL_MASK = (KeyCode)0x0100;
    public const KeyCode SHIFT_MASK = (KeyCode)0x0200;
    public const KeyCode ALT_MASK = (KeyCode)0x0400;
    public const KeyCode WIN_MASK = (KeyCode)0x0800;
    public const KeyCode SEND_ONCE_MASK = (KeyCode)0x1000;

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
      Plus = 0xBB,
      Comma = 0xBC,
      Minus = 0xBD,
      Period = 0xBE
    };
  }
}
