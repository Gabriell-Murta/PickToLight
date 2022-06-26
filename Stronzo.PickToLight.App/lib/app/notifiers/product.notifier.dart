import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:src/app/models/product.model.dart';
import 'package:src/app/providers/repository_providers.dart';

class ProductNotifier extends StateNotifier<AsyncValue<List<Product>>> {
  final Reader read;

  ProductNotifier(this.read) : super(AsyncValue.data(List.empty()));

  Future<void> getProducts(String clientId) async {
    try {
      state = const AsyncValue.loading();
      state = AsyncValue.data(await read(productRepositoryProvider).getProductsByClientId(clientId));
    } catch (e) {
      state = AsyncValue.error(e.toString());
    }
  }
}
