namespace task3
{
    using System.IO;

    string pathToFile = "/Users/vladshcherbyna/RiderProjects/task3/dictionary.txt";

    try
    {
        string[] lines = File.ReadAllLines(pathToFile);

        foreach (string line in lines)
        {
            Console.WriteLine(line);
            // do something with each line here
        }
    }
    catch (FileNotFoundException)
    {
        Console.WriteLine($"File not found: {pathToFile}");
    }
    catch (IOException ex)
    {
        Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
    }
    public class KeyValuePair
    {
        public string Key { get; }

        public string Value { get; }

        public KeyValuePair(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
    public class LinkedListNode
    {
        public KeyValuePair Pair { get; }

        public LinkedListNode Next { get; set; }

        public LinkedListNode(KeyValuePair pair, LinkedListNode next = null)
        {
            Pair = pair;
            Next = next;
        }
    }

    public class LinkedList
    {
        private LinkedListNode _first;

        public void Add(KeyValuePair pair)
        {
            if (_first == null)
            {
                _first = new LinkedListNode(pair);
            }
            else
            {
                LinkedListNode current = _first;
                while (current.Next != null)
                {
                    current = current.Next;
                }

                current.Next = new LinkedListNode(pair);
            }
        }

        public void RemoveByKey(string key)
        {
            if (_first == null)
            {
                return;
            }

            if (_first.Pair.Key == key)
            {
                _first = _first.Next;
                return;
            }

            LinkedListNode current = _first;
            while (current.Next != null)
            {
                if (current.Next.Pair.Key == key)
                {
                    current.Next = current.Next.Next;
                    return;
                }

                current = current.Next;
            }
        }

        public KeyValuePair GetItemWithKey(string key)
        {
            LinkedListNode current = _first;
            while (current != null)
            {
                if (current.Pair.Key == key)
                {
                    return current.Pair;
                }

                current = current.Next;
            }

            return null;
        }
    }
    public class StringsDictionary
    {
        private const int InitialSize = 10;

        private LinkedList[] _buckets = new LinkedList[InitialSize];

        public void Add(string key, string value)
        {
            int hash = CalculateHash(key);
            if (_buckets[hash] == null)
            {
                _buckets[hash] = new LinkedList();
            }
            _buckets[hash].Add(new KeyValuePair(key, value));
        }

        public void Remove(string key)
        {
            int hash = CalculateHash(key);
            if (_buckets[hash] != null)
            {
                LinkedListNode current = _buckets[hash].First;
                while (current != null)
                {
                    if (current.Value.Key == key)
                    {
                        _buckets[hash].Remove(current);
                        return;
                    }
                    current = current.Next;
                }
            }
        }

        public string Get(string key)
        {
            int hash = CalculateHash(key);
            if (_buckets[hash] != null)
            {
                LinkedListNode current = _buckets[hash].First;
                while (current != null)
                {
                    if (current.Value.Key == key)
                    {
                        return current.Value.Value;
                    }
                    current = current.Next;
                }
            }
            return null;
        }

        private int CalculateHash(string key)
        {
            const int p = 31; // prime number for hash function
            const int m = 1000000007; // large prime number for modulo operation
            int hash = 0;
            int p_pow = 1;
            foreach (char c in key)
            {
                hash = (hash + (c - 'a' + 1) * p_pow) % m;
                p_pow = (p_pow * p) % m;
            }
            return hash % _buckets.Length;
        }
    }
}