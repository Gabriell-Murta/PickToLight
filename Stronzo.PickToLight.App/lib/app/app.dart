import 'package:flutter/material.dart';
import 'package:src/app/views/login.view.dart';

class MyApp extends StatelessWidget {
  const MyApp({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Stronzo - Pick to Light',
      debugShowCheckedModeBanner: false,
      theme: ThemeData(
        colorScheme: const ColorScheme.dark(),
        primarySwatch: Colors.orange,
      ),
      home: LoginPage(),
    );
  }
}
