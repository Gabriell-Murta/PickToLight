import 'package:flutter/material.dart';
import 'package:flutter_blue_plus/flutter_blue_plus.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:src/app/models/product.model.dart';
import 'package:src/app/providers/providers.dart';
import 'package:src/app/repositories/product.repository.dart';
import 'package:src/app/views/available_devices_list.view.dart';
import 'package:src/shared/extensions/async_value_extensions.dart';
import 'package:src/shared/helpers/bluetooth_scan_helper.dart';

class ProductsList extends ConsumerStatefulWidget {
  const ProductsList({Key? key}) : super(key: key);

  @override
  ConsumerState<ConsumerStatefulWidget> createState() => _ProductsListState();
}

class _ProductsListState extends ConsumerState<ProductsList> {
  bool _isFindingProduct = false;

  _fetchProducts() {
    final userData = ref.read(userProvider);
    return ref.read(productProvider.notifier).getProducts(userData.value!.clientId);
  }

  _showLinkDeviceDialog() {
    beginScan();
    showDialog(
      context: context,
      barrierDismissible: true,
      builder: (context) => AlertDialog(
        scrollable: false,
        title: Row(
          crossAxisAlignment: CrossAxisAlignment.center,
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            const Text("Vincular dispositivo"),
            StreamBuilder<bool>(
              stream: FlutterBluePlus.instance.isScanning,
              initialData: false,
              builder: (c, snapshot) {
                if (!snapshot.hasData) return const SizedBox.shrink();

                var isScanning = snapshot.data!;
                return isScanning
                    ? const Center(child: CircularProgressIndicator())
                    : IconButton(
                        onPressed: () => beginScan(),
                        icon: const Icon(Icons.refresh),
                      );
              },
            ),
          ],
        ),
        content: const SizedBox.expand(child: AvailableDevicesList(unconfiguredDevices: false)),
      ),
    );
  }

  _findProduct(Product product) async {
    setState(() {
      _isFindingProduct = true;
    });

    await ProductRepository(ref.read).findProduct(product.id);

    setState(() {
      _isFindingProduct = false;
    });
  }

  _buildListItem(Product product) => ListTile(
        title: Text(product.name),
        subtitle: Text(product.code),
        trailing: IconButton(
          onPressed: () => _showLinkDeviceDialog(),
          icon: const Icon(Icons.add),
        ),
        onTap: () => _findProduct(product),
      );

  @override
  Widget build(BuildContext context) {
    final productsList = ref.watch(productProvider);

    ref.listen<AsyncValue<List<Product>>>(productProvider, (_, state) => state.showSnackBarOnError(context));

    return RefreshIndicator(
      onRefresh: () => _fetchProducts(),
      child: productsList.isLoading || _isFindingProduct
          ? const Center(child: CircularProgressIndicator())
          : ListView.separated(
              separatorBuilder: ((context, index) => const Divider()),
              itemBuilder: (context, index) => _buildListItem(productsList.value![index]),
              itemCount: productsList.value?.length ?? 0,
            ),
    );
  }
}
