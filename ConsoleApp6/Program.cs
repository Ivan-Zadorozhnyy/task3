namespace task3
{
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

        public LinkedListNode First => _first;

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
                    current.Next = new LinkedListNode(current.Next.Pair, current.Next.Next);
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
                    return new LinkedListNode(current.Pair, current.Next).Pair;
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
            if (hash == _buckets.Length - 1)
            {
                // Double the size of the array and re-allocate the elements
                LinkedList[] newBuckets = new LinkedList[InitialSize * 2];
                Array.Copy(_buckets, newBuckets, _buckets.Length);
                _buckets = newBuckets;
            }
        }

        public void Remove(string key)
        {
            int hash = CalculateHash(key);
            if (_buckets[hash] != null)
            {
                LinkedListNode current = _buckets[hash].First;
                while (current != null)
                {
                    if (current.Pair.Key == key)
                    {
                        _buckets[hash].RemoveByKey(key);
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
                    if (current.Pair.Key == key)
                    {
                        return current.Pair.Value;
                    }

                    current = current.Next;
                }
            }

            return null;
        }

        private int CalculateHash(string key)
        {
            int hash = key.GetHashCode();
            return (hash & 0x7FFFFFFF) % InitialSize;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            string filePath = "/Users/ivanzadorozhnyy/RiderProjects/ConsoleApp6/ConsoleApp6/dictionary.txt";
            StringsDictionary stringsDictionary = new StringsDictionary();
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] words = line.Split(";");
                    stringsDictionary.Add(words.First(), words.Last());
                }
            }

            while (true)
            {
                Console.Write("Enter a word (or type 'exit' to quit): ");
                string input = Console.ReadLine();
                if (input == "exit")
                {
                    break;
                }

                string value = stringsDictionary.Get(input);
                Console.WriteLine($"Meaning of {input}: {value}");
            }
        }
    }
}