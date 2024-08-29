using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateFields.ShoppingCart;

public readonly record struct ItemId(Guid itemId) 
{
    public static ItemId NewId => new(Guid.NewGuid());
    public static ItemId Empty => new(Guid.Empty);
}

internal class Item
{
    private readonly ItemId _itemId = ItemId.NewId;
    private string? _itemName;
    private decimal _price;


    public Item(string itemName, decimal price, uint quantityInStock)
    {
        ItemName = itemName;
        Price = price;
        QuantityInStock = quantityInStock;
    }

    public ItemId Id { get => _itemId; } 

    public string? ItemName
    {
        get => _itemName;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value), "Can't be empty or whitespace");
            else
            {
                _itemName = value;
            }
        }
    }

    public decimal Price
    {
        get => _price;
        set 
        {
            if (value < 0) throw new ArgumentException("Price can't be a negative number");
            else 
            {
                _price = value;
            }
        }
    }

    public uint QuantityInStock { get; set; }

    public override string ToString()
    {
        return
    $@"
    Id: {Id} 
    ItemName: {ItemName} 
    Price: {Price:C} 
    QuantityInStock: {QuantityInStock}";
    }
}
