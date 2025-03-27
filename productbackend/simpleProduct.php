<?php
header('Content-Type: application/json');

$product = [
  "Name" => "Product X",
  "Description" => "Dit is een voorbeeldproduct"
];

echo json_encode($product);
?>
