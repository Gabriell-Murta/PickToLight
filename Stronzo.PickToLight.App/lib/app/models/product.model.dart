class Product {
  final String id;
  final String name;
  final String code;

  Product({
    required this.name,
    required this.code,
    required this.id,
  });

  factory Product.fromJson(Map<String, dynamic> json) => Product(
        id: json['id'],
        name: json['name'],
        code: json['code'],
      );

  Map<String, dynamic> toJson() => <String, dynamic>{'id': id, 'name': name, 'code': code};
}
