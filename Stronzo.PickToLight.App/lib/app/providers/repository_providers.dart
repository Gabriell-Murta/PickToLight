import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:src/app/repositories/device.repository.dart';
import 'package:src/app/repositories/product.repository.dart';
import 'package:src/app/repositories/user.repository.dart';

final deviceRepositoryProvider = Provider((ref) => DeviceRepository(ref.read));
final productRepositoryProvider = Provider((ref) => ProductRepository(ref.read));
final userRepositoryProvider = Provider((ref) => UserRepository(ref.read));
