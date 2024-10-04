using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace PrivateFields.ShoppingCart;

public readonly record struct ShoppingCartId(Guid shoppingCartId) 
{
    public static ShoppingCartId NewId => new(Guid.NewGuid());
    public static ShoppingCartId Empty => new(Guid.Empty);  
};

internal class ShoppingCart
{
    private readonly ShoppingCartId _shoppingCartId;
    private readonly List<Item> _items = [];
    private readonly object _lock = new();

    public ShoppingCart()
    {
        _shoppingCartId = ShoppingCartId.NewId;
    }

    public ShoppingCartId ShoppingCartId { get => _shoppingCartId; }

    public List<Item> Items
    {
        get
        {
            lock (_lock) 
            {
                return _items;
            }
        }
     
    }

    public decimal TotalPrice
    {
        get
        {
            lock (_lock) 
            {
                return _items.Sum(item => item.Price);
            }
        }      
    }

    public void AddItem(Item item) 
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item), "Item cannot be null.");

        lock (_lock)
        {
            if (item.QuantityInStock < 1)
            {
                Console.WriteLine("Out of stock!");
            }
            else
            {
                Items.Add(item);
                item.QuantityInStock--;
            }
        }
    }

    public void RemoveItem(Item item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item), "Item cannot be null.");
        lock (_lock)
        {
            if (_items.Remove(item))
            {
                item.QuantityInStock++;
            }
            else
            {
                Console.WriteLine("Item not found in the cart");
            }
        }
    }

    public void Checkout()
    {
        lock (_lock)
        {
            if (_items.Count == 0)
            {
                Console.WriteLine("\nCart is empty!");

            }
            else
            {
                Console.WriteLine($"\nThank you for your purchase of {TotalPrice:C} and not paying for any of it");
                _items.Clear();
            }
        }
    }

    public void DisplayCartItems()
    {
        lock (_lock)
        {
            Console.WriteLine("\nCart contents:");
            foreach (var item in Items)
            {
                Console.WriteLine(item);
            }
        }
    }
}
