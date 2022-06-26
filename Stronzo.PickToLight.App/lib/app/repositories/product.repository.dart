import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:src/app/models/product.model.dart';
import 'package:src/shared/repository_base.dart';

class ProductRepository extends ProviderRepositoryBase {
  ProductRepository(Reader read) : super(read);

  Future<List<Product>> getProductsByClientId(String clientId) async {
    var response = await dio.get<List<dynamic>>('/api/v1/products?clientId=$clientId');

    if (response.statusCode! >= 200 && response.statusCode! < 300) {
      return response.data!.map((element) => Product.fromJson(element)).toList();
    }

    throw Exception('Não foi possível obter os produtos');
  }

  Future<void> findProduct(String productId) async {
    var response = await dio.post('/api/v1/products/$productId');

    if (response.statusCode! >= 200 && response.statusCode! < 300) {
      return;
    }

    throw Exception("Não foi possível ativar o dispositivo vinculado ao produto :(");
  }
}
