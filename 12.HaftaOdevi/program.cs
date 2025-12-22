using System;
using System.Collections.Generic;

namespace AVL_DSW_SelfAdjusting
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== AVL - DSW - Self Adjusting Tree ===\n");

            char[] chars = { 'S', 'E', 'L', 'İ', 'M', 'K', 'A', 'Ç', 'T', 'I' };

            // 1. AVL Tree
            Console.WriteLine("1. AVL AĞACI:");
            AVLTree avl = new AVLTree();
            foreach (char c in chars) avl.Insert(c);
            avl.PrintTree();

            // 2. DSW
            Console.WriteLine("\n2. DSW ALGORİTMASI:");
            Console.WriteLine("Amaç: Ağaç yüksekliğini minimuma indirmek");
            Console.WriteLine("Geliştirenler: Colin Day, Quentin F. Stout, Bette L. Warren\n");

            DSWTree dsw = new DSWTree(avl.Root);
            Console.WriteLine("DSW Öncesi:");
            dsw.PrintTree();

            dsw.ApplyDSW();
            Console.WriteLine("\nDSW Sonrası:");
            dsw.PrintTree();

            // 3. Self-Adjusting Tree
            Console.WriteLine("\n3. SELF-ADJUSTING TREE:");
            SelfAdjustingTree sat = new SelfAdjustingTree(avl.Root);

            Console.WriteLine("Arama işlemleri:");
            sat.Search('A');
            sat.Search('L');
            sat.Search('L');

            sat.PrintTree();
        }
    }

    class TreeNode
    {
        public char Data;
        public TreeNode Left, Right;
        public int Height, Frequency;
        public TreeNode Parent;

        public TreeNode(char data, TreeNode parent = null)
        {
            Data = data;
            Height = 1;
            Frequency = 0;
            Parent = parent;
        }
    }

    // AVL Tree
    class AVLTree
    {
        public TreeNode Root;

        public void Insert(char data) => Root = Insert(Root, data);

        private TreeNode Insert(TreeNode node, char data)
        {
            if (node == null) return new TreeNode(data);

            if (data < node.Data)
                node.Left = Insert(node.Left, data);
            else if (data > node.Data)
                node.Right = Insert(node.Right, data);
            else return node;

            node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));
            int balance = GetBalance(node);

            // Rotations
            if (balance > 1 && data < node.Left.Data) // LL
                return RotateRight(node);
            if (balance < -1 && data > node.Right.Data) // RR
                return RotateLeft(node);
            if (balance > 1 && data > node.Left.Data) // LR
            {
                node.Left = RotateLeft(node.Left);
                return RotateRight(node);
            }
            if (balance < -1 && data < node.Right.Data) // RL
            {
                node.Right = RotateRight(node.Right);
                return RotateLeft(node);
            }

            return node;
        }

        private int Height(TreeNode node) => node?.Height ?? 0;

        private int GetBalance(TreeNode node) => node == null ? 0 : Height(node.Left) - Height(node.Right);

        private TreeNode RotateRight(TreeNode y)
        {
            Console.WriteLine($"  RR Rotation on {y.Data}");
            var x = y.Left;
            var T2 = x.Right;

            x.Right = y;
            y.Left = T2;

            y.Height = Math.Max(Height(y.Left), Height(y.Right)) + 1;
            x.Height = Math.Max(Height(x.Left), Height(x.Right)) + 1;

            return x;
        }

        private TreeNode RotateLeft(TreeNode x)
        {
            Console.WriteLine($"  LL Rotation on {x.Data}");
            var y = x.Right;
            var T2 = y.Left;

            y.Left = x;
            x.Right = T2;

            x.Height = Math.Max(Height(x.Left), Height(x.Right)) + 1;
            y.Height = Math.Max(Height(y.Left), Height(y.Right)) + 1;

            return y;
        }

        public void PrintTree() => PrintTree(Root, "", true);

        private void PrintTree(TreeNode node, string indent, bool last)
        {
            if (node == null) return;

            Console.Write(indent + (last ? "└─" : "├─") + node.Data + $"(h:{node.Height})");
            if (node.Left == null && node.Right == null) Console.Write(" [LEAF]");
            Console.WriteLine();

            PrintTree(node.Left, indent + (last ? "  " : "│ "), false);
            PrintTree(node.Right, indent + (last ? "  " : "│ "), true);
        }
    }

    // DSW Tree
    class DSWTree
    {
        private TreeNode root;
        private int count;

        public DSWTree(TreeNode avlRoot)
        {
            root = CopyTree(avlRoot);
            count = CountNodes(root);
        }

        private TreeNode CopyTree(TreeNode node)
        {
            if (node == null) return null;
            var newNode = new TreeNode(node.Data);
            newNode.Left = CopyTree(node.Left);
            newNode.Right = CopyTree(node.Right);
            return newNode;
        }

        private int CountNodes(TreeNode node) => node == null ? 0 : 1 + CountNodes(node.Left) + CountNodes(node.Right);

        public void ApplyDSW()
        {
            CreateBackbone();
            BalanceBackbone();
        }

        private void CreateBackbone()
        {
            Console.WriteLine("  Creating backbone...");
            var temp = new TreeNode('\0');
            temp.Right = root;
            var tail = temp;
            var current = temp.Right;

            while (current != null)
            {
                if (current.Left != null)
                {
                    var child = current.Left;
                    current.Left = child.Right;
                    child.Right = current;
                    current = child;
                    tail.Right = child;
                }
                else
                {
                    tail = current;
                    current = current.Right;
                }
            }
            root = temp.Right;
        }

        private void BalanceBackbone()
        {
            Console.WriteLine("  Balancing backbone...");
            int m = (int)Math.Pow(2, Math.Floor(Math.Log(count + 1, 2))) - 1;

            // First phase
            var temp = new TreeNode('\0');
            temp.Right = root;
            var current = temp;

            for (int i = 0; i < count - m; i++)
            {
                var child = current.Right;
                current.Right = child.Right;
                current = current.Right;
                child.Right = current.Left;
                current.Left = child;
            }
            root = temp.Right;

            // Second phase
            while (m > 1)
            {
                m /= 2;
                temp = new TreeNode('\0');
                temp.Right = root;
                current = temp;

                for (int i = 0; i < m; i++)
                {
                    var child = current.Right;
                    current.Right = child.Right;
                    current = current.Right;
                    child.Right = current.Left;
                    current.Left = child;
                }
                root = temp.Right;
            }
        }

        public void PrintTree() => PrintTree(root, "", true);

        private void PrintTree(TreeNode node, string indent, bool last)
        {
            if (node == null) return;
            Console.WriteLine(indent + (last ? "└─" : "├─") + node.Data);
            PrintTree(node.Left, indent + (last ? "  " : "│ "), false);
            PrintTree(node.Right, indent + (last ? "  " : "│ "), true);
        }
    }

    // Self-Adjusting Tree
    class SelfAdjustingTree
    {
        private TreeNode root;

        public SelfAdjustingTree(TreeNode avlRoot)
        {
            root = CopyTree(avlRoot, null);
        }

        private TreeNode CopyTree(TreeNode node, TreeNode parent)
        {
            if (node == null) return null;
            var newNode = new TreeNode(node.Data, parent);
            newNode.Left = CopyTree(node.Left, newNode);
            newNode.Right = CopyTree(node.Right, newNode);
            return newNode;
        }

        public void Search(char data)
        {
            var node = FindNode(root, data);
            if (node != null)
            {
                node.Frequency++;
                Console.WriteLine($"  Found '{data}'. Frequency: {node.Frequency}");
                AdjustToRoot(node);
            }
        }

        private TreeNode FindNode(TreeNode node, char data)
        {
            if (node == null || node.Data == data) return node;
            return data < node.Data ? FindNode(node.Left, data) : FindNode(node.Right, data);
        }

        private void AdjustToRoot(TreeNode node)
        {
            while (node.Parent != null)
            {
                var parent = node.Parent;
                var grandParent = parent.Parent;

                if (parent.Left == node) // Right rotation
                {
                    Console.WriteLine($"    Right rotate {node.Data}");
                    parent.Left = node.Right;
                    if (node.Right != null) node.Right.Parent = parent;
                    node.Right = parent;
                    parent.Parent = node;
                }
                else // Left rotation
                {
                    Console.WriteLine($"    Left rotate {node.Data}");
                    parent.Right = node.Left;
                    if (node.Left != null) node.Left.Parent = parent;
                    node.Left = parent;
                    parent.Parent = node;
                }

                node.Parent = grandParent;
                if (grandParent != null)
                {
                    if (grandParent.Left == parent) grandParent.Left = node;
                    else grandParent.Right = node;
                }
                else root = node;

            }
        }

        public void PrintTree()
        {
            Console.WriteLine("Self-Adjusting Tree:");
            PrintTree(root, "", true);
        }

        private void PrintTree(TreeNode node, string indent, bool last)
        {
            if (node == null) return;
            Console.WriteLine(indent + (last ? "└─" : "├─") + node.Data + $"(f:{node.Frequency})");
            PrintTree(node.Left, indent + (last ? "  " : "│ "), false);
            PrintTree(node.Right, indent + (last ? "  " : "│ "), true);
        }
    }
}
