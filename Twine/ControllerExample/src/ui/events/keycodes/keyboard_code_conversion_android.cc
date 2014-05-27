// Copyright 2013 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

#include "ui/events/keycodes/keyboard_code_conversion_android.h"

#include <android/keycodes.h>

namespace ui {

namespace {

// The Android NDK does not provide values for these yet:
enum {
  AKEYCODE_MEDIA_REWIND    = 89,
  AKEYCODE_MEDIA_FAST_FORWARD = 90,
  AKEYCODE_ESCAPE          = 111,
  AKEYCODE_FORWARD_DEL     = 112,
  AKEYCODE_CTRL_LEFT       = 113,
  AKEYCODE_CTRL_RIGHT      = 114,
  AKEYCODE_CAPS_LOCK       = 115,
  AKEYCODE_SCROLL_LOCK     = 116,
  AKEYCODE_META_LEFT       = 117,
  AKEYCODE_META_RIGHT      = 118,
  AKEYCODE_BREAK           = 121,
  AKEYCODE_INSERT          = 124,
  AKEYCODE_MEDIA_PLAY      = 126,
  AKEYCODE_MEDIA_PAUSE     = 127,
  AKEYCODE_F1              = 131,
  AKEYCODE_F2              = 132,
  AKEYCODE_F3              = 133,
  AKEYCODE_F4              = 134,
  AKEYCODE_F5              = 135,
  AKEYCODE_F6              = 136,
  AKEYCODE_F7              = 137,
  AKEYCODE_F8              = 138,
  AKEYCODE_F9              = 139,
  AKEYCODE_F10             = 140,
  AKEYCODE_F11             = 141,
  AKEYCODE_F12             = 142,
  AKEYCODE_NUM_LOCK        = 143,
  AKEYCODE_NUMPAD_0        = 144,
  AKEYCODE_NUMPAD_1        = 145,
  AKEYCODE_NUMPAD_2        = 146,
  AKEYCODE_NUMPAD_3        = 147,
  AKEYCODE_NUMPAD_4        = 148,
  AKEYCODE_NUMPAD_5        = 149,
  AKEYCODE_NUMPAD_6        = 150,
  AKEYCODE_NUMPAD_7        = 151,
  AKEYCODE_NUMPAD_8        = 152,
  AKEYCODE_NUMPAD_9        = 153,
  AKEYCODE_NUMPAD_DIVIDE   = 154,
  AKEYCODE_NUMPAD_MULTIPLY = 155,
  AKEYCODE_NUMPAD_SUBTRACT = 156,
  AKEYCODE_NUMPAD_ADD      = 157,
  AKEYCODE_NUMPAD_DOT      = 158,
  AKEYCODE_VOLUME_MUTE     = 164,
  AKEYCODE_CHANNEL_UP      = 166,
  AKEYCODE_CHANNEL_DOWN    = 167,
};

}  // namespace

KeyboardCode KeyboardCodeFromAndroidKeyCode(int keycode) {
  return (KeyboardCode)keycode;
}

}  // namespace ui
