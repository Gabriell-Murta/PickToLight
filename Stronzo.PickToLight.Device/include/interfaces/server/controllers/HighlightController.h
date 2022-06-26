#pragma once

#include "interfaces/helpers/IndicationStateHelper.h"
#include "utils/BluetoothUtils.h"

class HighlightController {
 public:
  static HighlightHandler highlight;
};

HighlightHandler HighlightController::highlight = []() {
  log_d("Received highlight request");

  indicationHelper.setState(IndicationState::Highlight);
};
