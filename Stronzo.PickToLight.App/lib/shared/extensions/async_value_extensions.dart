import 'package:flutter/material.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:src/shared/helpers/snack_bar_helper.dart';

extension AsyncValueUI on AsyncValue<dynamic> {
  bool get isLoading => this is AsyncLoading<dynamic>;

  void showSnackBarOnError(BuildContext context) => whenOrNull(error: (error, _) => showErrorSnackBar(context, error));
}
