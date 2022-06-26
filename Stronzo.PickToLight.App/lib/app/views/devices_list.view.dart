import 'package:collection/collection.dart';
import 'package:flutter/material.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:src/app/models/device.model.dart';
import 'package:src/app/providers/providers.dart';
import 'package:src/shared/extensions/async_value_extensions.dart';

class DevicesList extends ConsumerStatefulWidget {
  const DevicesList({Key? key}) : super(key: key);

  @override
  ConsumerState<ConsumerStatefulWidget> createState() => _DevicesListState();
}

class _DevicesListState extends ConsumerState<DevicesList> {
  _fetchDevices() {
    final userData = ref.read(userProvider);
    return ref.read(deviceProvider.notifier).getDevices(userData.value!.clientId);
  }

  _buildListItem(Device device) => ListTile(
        title: Text(device.id),
        subtitle: Text(
            ref.read(productProvider).value!.firstWhereOrNull((product) => product.id == device.productId)?.name ?? ""),
      );

  @override
  Widget build(BuildContext context) {
    final devicesList = ref.watch(deviceProvider);

    ref.listen<AsyncValue<List<Device>>>(deviceProvider, (_, state) => state.showSnackBarOnError(context));

    return RefreshIndicator(
      onRefresh: () => _fetchDevices(),
      child: devicesList.isLoading
          ? const Center(child: CircularProgressIndicator())
          : ListView.separated(
              separatorBuilder: ((context, index) => const Divider()),
              itemBuilder: (context, index) => _buildListItem(devicesList.value![index]),
              itemCount: devicesList.value?.length ?? 0,
            ),
    );
  }
}
