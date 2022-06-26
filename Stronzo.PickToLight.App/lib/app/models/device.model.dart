class Device {
  final String id;
  final String? productId;

  Device({required this.id, required this.productId});

  factory Device.fromJson(Map<String, dynamic> json) => Device(id: json['id'], productId: json['productId']);

  Map<String, dynamic> toJson() => <String, dynamic>{'id': id, 'productId': productId};
}
