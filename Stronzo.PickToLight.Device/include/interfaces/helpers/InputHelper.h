#pragma once

#include <Arduino.h>
#include <stdlib.h>
#include <functional>

typedef std::function<void()> ParameterLessCallback;

class InputHelper {
 private:
  ParameterLessCallback _onTouch;
  ParameterLessCallback _onLongTouch;

 public:
  InputHelper() {}

  void setOnTouch(ParameterLessCallback callback) { _onTouch = callback; }
  void setOnLongTouch(ParameterLessCallback callback) { _onLongTouch = callback; }

  void begin() {}

  void loop() {}
};
