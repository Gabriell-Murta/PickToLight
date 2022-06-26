#pragma once

#include <Arduino.h>

enum IndicationState {
  Idle = 1,
  Active = 2,
  Disconnected = 3,
  Configuration = 4,
  Highlight = 5,
  LinkMode = 6,
};

const char* getStateString(IndicationState state) {
  switch (state) {
    case Idle:
      return "Idle";
    case Active:
      return "Active";
    case Disconnected:
      return "Disconnected";
    case Configuration:
      return "Configuration";
    case Highlight:
      return "Highlight";
    case LinkMode:
      return "LinkMode";
    default:
      return "Undefined";
  }
}

class IndicationStateHelper {
 private:
  uint32_t _animationTimeout = 0;
  uint32_t _ledMillis = 0;
  bool _currentLedState = false;

  IndicationState _previousIndicationState = IndicationState::Idle;
  IndicationState _currentIndicationState = IndicationState::Idle;

  void _handleIdle() {
    // turn off leds
    digitalWrite(LED_BUILTIN, LOW);
  }

  void _handleActive() {
    // do led animation
    if ((millis() - _ledMillis) > 500) {
      digitalWrite(LED_BUILTIN, _currentLedState);
      _currentLedState = !_currentLedState;
      _ledMillis = millis();
    }
  }

  void _handleDisconnected() {
    // do disconnected led animation
    if ((millis() - _ledMillis) > 100) {
      digitalWrite(LED_BUILTIN, _currentLedState);
      _currentLedState = !_currentLedState;
      _ledMillis = millis();
    }
  }

  void _handleConfiguration() {
    // do configuration led animation
    if ((millis() - _ledMillis) > 1000) {
      digitalWrite(LED_BUILTIN, _currentLedState);
      _currentLedState = !_currentLedState;
      _ledMillis = millis();
    }
  }

  void _handleHighlight() {
    if ((millis() - _ledMillis) > 50) {
      digitalWrite(LED_BUILTIN, _currentLedState);
      _currentLedState = !_currentLedState;
      _ledMillis = millis();
    }

    if ((millis() - _animationTimeout) > 2000) {
      _revertToPrevious();
    }
  }

  void _revertToPrevious() { _currentIndicationState = _previousIndicationState; }

 public:
  void setState(IndicationState state) {
    log_d("Setting current state to: %s", getStateString(state));

    _animationTimeout = millis();

    _previousIndicationState = _currentIndicationState;
    _currentIndicationState = state;
  }

  void begin() { pinMode(LED_BUILTIN, OUTPUT); }

  void loop() {
    switch (_currentIndicationState) {
      case Idle:
        _handleIdle();
        break;
      case Active:
        _handleActive();
        break;
      case Disconnected:
        _handleDisconnected();
        break;
      case Configuration:
        _handleConfiguration();
        break;
      case Highlight:
        _handleHighlight();
        break;
      default:
        log_e("Invalid indication state provided: %d", _currentIndicationState);
        break;
    }
  }
};

static IndicationStateHelper indicationHelper;
