import 'package:flutter/material.dart';

showErrorSnackBar(BuildContext context, Object error) =>
    ScaffoldMessenger.of(context).showSnackBar(SnackBar(content: Text(error.toString())));
