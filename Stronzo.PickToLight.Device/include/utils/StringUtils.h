#pragma once

#include <Arduino.h>
#include <stdlib.h>

template <typename... Args>
std::string string_format(const std::string& format, Args... args) {
  auto size = snprintf(nullptr, 0, format.c_str(), args...) + 1;
  if (size <= 0) {
    return nullptr;
  }
  std::unique_ptr<char[]> buf(new char[size]);
  snprintf(buf.get(), size, format.c_str(), args...);
  return std::string(buf.get(), buf.get() + size - 1);
}

template <const unsigned num, const char separator>
void string_tokenize(std::string& input) {
  for (auto it = input.begin(); (num + 1) <= std::distance(it, input.end()); ++it) {
    std::advance(it, num);
    it = input.insert(it, separator);
  }
}
