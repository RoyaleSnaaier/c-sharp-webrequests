<?php
class Product implements \JsonSerializable
{
	public string $name;
	public float $price;  // Changed from int to float
	public string $shortDescription;
	public int $productType;

	public function __construct(
		string $name,
		float $price,  // accept floats in constructor
		string $shortDescription,
		int $productType
	) {
		$this->name = $name;
		$this->price = $price;
		$this->shortDescription = $shortDescription;
		$this->productType = $productType;
	}

	public static function fromJson(\stdClass $data): self
	{
		return new self(
			$data->Name,
			(float)$data->Price,
			$data->ShortDescription,
			$data->ProductType
		);
	}

	// Explicitly define jsonSerialize to ensure correct JSON structure
	public function jsonSerialize()
{
    return [
        'Name' => $this->name,
        'Price' => round($this->price, 2),  // clearly numeric, NOT string
        'ShortDescription' => $this->shortDescription,
        'ProductType' => $this->productType
    ];
}

}
