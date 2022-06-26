import 'package:flutter/material.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:src/app/models/device.model.dart';
import 'package:src/app/models/product.model.dart';
import 'package:src/app/providers/providers.dart';
import 'package:src/app/views/available_devices_page.view.dart';
import 'package:src/app/views/devices_page.view.dart';
import 'package:src/app/views/products_page.view.dart';
import 'package:src/shared/extensions/async_value_extensions.dart';

class MainPage extends ConsumerStatefulWidget {
  const MainPage({Key? key}) : super(key: key);

  @override
  ConsumerState<ConsumerStatefulWidget> createState() => _MainPageState();
}

class _MainPageState extends ConsumerState<MainPage> {
  @override
  Widget build(BuildContext context) {
    final userState = ref.watch(userProvider);

    ref.listen<AsyncValue<List<Device>>>(deviceProvider, (_, state) {
      state.showSnackBarOnError(context);
    });

    ref.listen<AsyncValue<List<Product>>>(productProvider, (_, state) {
      state.showSnackBarOnError(context);
    });

    return userState.isLoading
        ? const CircularProgressIndicator()
        : const DefaultTabController(
            length: 3,
            child: Scaffold(
              body: TabBarView(children: [
                ProductsPage(),
                DevicesPage(),
                AvailableDevicesPage(),
              ]),
              bottomNavigationBar: TabBar(tabs: [
                Tab(icon: Icon(Icons.list)),
                Tab(icon: Icon(Icons.devices)),
                Tab(icon: Icon(Icons.engineering)),
              ]),
            ),
          );
  }
}
