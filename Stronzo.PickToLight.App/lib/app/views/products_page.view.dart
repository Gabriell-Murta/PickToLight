import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:src/app/views/products_list.view.dart';

class ProductsPage extends StatefulWidget {
  const ProductsPage({Key? key}) : super(key: key);

  @override
  State<ProductsPage> createState() => _ProductsPageState();
}

class _ProductsPageState extends State<ProductsPage> {
  final _searchInputController = TextEditingController(text: '');

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        actions: const [],
        title: TextField(
          controller: _searchInputController,
          inputFormatters: [FilteringTextInputFormatter.allow(RegExp(r'[0-9a-zA-Z]+'))],
          onChanged: (value) {},
          style: const TextStyle(color: Colors.white),
          decoration: const InputDecoration(
            hintText: 'Insira o nome ou código do produto',
            hintStyle: TextStyle(
              fontSize: 16,
              fontStyle: FontStyle.normal,
            ),
          ),
        ),
      ),
      body: const ProductsList(),
    );
  }
}
