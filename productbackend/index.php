<?php
require_once 'models/Product.php';

if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    
    // get json data
    $json = file_get_contents('php://input');
    if ($data = json_decode($json, true)) {
        // validate data

        $postedProduct = Product::fromJson((object) $data);

        // return product
        new Response('Product added to database', $postedProduct, 201);

    } 
} else if ($_SERVER['REQUEST_METHOD'] == 'GET') {
    // set status code 

    // idk, return a bunch of products
    $products = [
        new Product('Product 1', 100.00, 'This is a product', 1),
        new Product('Product 2', 20.52, 'This is another product', 2),
        new Product('Product 3', 6.20, 'This is yet another product', 1),
        new Product('Product 4', 42.0, 'This is the last product', 1),
    ];

    // return products
    new Response('Success', $products);
} else {
    new Response('Error', 'Invalid request method', 405);
}

class Response
{
    public $Message;
    public $Data;
    function __construct($message, $data, $status = 200)
    {
        $this->Message = $message;
        $this->Data = $data;
        // set status code
        http_response_code($status);
        $this->Send();
    }

    function Send()
    {
        // set headers to json
        header('Content-Type: application/json');
        echo json_encode($this);
    }
}
