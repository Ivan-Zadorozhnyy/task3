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
        private int _count;

        public LinkedListNode First => _first;
        public int Count => _count;

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
            _count++;
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
                _count--;
                return;
            }

            LinkedListNode current = _first;
            while (current.Next != null)
            {
                if (current.Next.Pair.Key == key)
                {
                    current.Next = new LinkedListNode(current.Next.Pair, current.Next.Next);
                    _count--;
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
        private const double LoadFactor = 0.75;

        private LinkedList[] _buckets = new LinkedList[InitialSize];

        public void Add(string key, string value)
        {
            int hash = CalculateHash(key);
            if (_buckets[hash] == null)
            {
                _buckets[hash] = new LinkedList();
            }

            _buckets[hash].Add(new KeyValuePair(key, value));
            if (hash == _buckets.Length - 1 && GetNumberOfElements() > _buckets.Length * LoadFactor)
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

        private int GetNumberOfElements()
        {
            int numberOfElements = 0;
            for (int i = 0; i < _buckets.Length; i++)
            {
                if (_buckets[i] != null)
                {
                    numberOfElements += _buckets[i].Count;
                }
            }

            return numberOfElements;
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
                Console.Write("Enter first word (or type 'exit' to quit): ");
                string word1 = Console.ReadLine();
                Console.Write("Enter second word: ");
                string word2 = Console.ReadLine();
                
                //Console.Write("Enter a word (or type 'exit' to quit): ");
                //string input = Console.ReadLine();
                
                if (word1.Length != word2.Length)
                {
                    Console.WriteLine("words are not anogramma");
                    return;
                }
                //if (word1 == "exit")
                //{
                //break;
                //}
                
                for (int i = 0; i < word1.Length; i++)
                {
                    for (int j = 0; j < word2.Length; j++)
                    {
                        if (word1[i] == word2[j])
                        {
                            break;
                        }
                        else if (j == word2.Length - 1)
                        {
                            Console.WriteLine("words are not anogramma");
                            return;
                        }
                    }
                }

                Console.WriteLine("words are anogramma");
            }

            //string value = stringsDictionary.Get(word1);
            //Console.WriteLine($"Meaning of {word1}: {value}");
        }
    }
}