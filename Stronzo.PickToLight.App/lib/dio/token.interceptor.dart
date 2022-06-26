import 'package:dio/dio.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:src/app/providers/providers.dart';

class TokenInterceptor extends Interceptor {
  final Reader read;

  TokenInterceptor(this.read);

  @override
  void onRequest(RequestOptions options, RequestInterceptorHandler handler) {
    options.headers.update(
      'Authorization',
      (value) => 'Bearer ${read(authProvider).value!.accessToken}',
      ifAbsent: () => 'Bearer ${read(authProvider).value!.accessToken}',
    );

    handler.next(options);
  }

  @override
  void onResponse(Response response, ResponseInterceptorHandler handler) {
    handler.next(response);
  }
}
