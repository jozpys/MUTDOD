using System;
using System.Linq;
using System.Collections.Generic;
using IndexPlugin;

/* Software License Agreement (BSD License)
* 
* Copyright (c) 2003, Herbert M Sauro
* All rights reserved.
*
* Redistribution and use in source and binary forms, with or without
* modification, are permitted provided that the following conditions are met:
*     * Redistributions of source code must retain the above copyright
*       notice, this list of conditions and the following disclaimer.
*     * Redistributions in binary form must reproduce the above copyright
*       notice, this list of conditions and the following disclaimer in the
*       documentation and/or other materials provided with the distribution.
*     * Neither the name of Herbert M Sauro nor the
*       names of its contributors may be used to endorse or promote products
*       derived from this software without specific prior written permission.
*
* THIS SOFTWARE IS PROVIDED BY <copyright holder> ``AS IS'' AND ANY
* EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
* WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
* DISCLAIMED. IN NO EVENT SHALL <copyright holder> BE LIABLE FOR ANY
* DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
* (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
* LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
* ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
* (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
* SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

namespace BasicIndexes.BinaryTree
{
    // A very basic Binary Search Tree. Not generalized, stores
    // name/value pairs in the tree nodes. name is the node key.
    // The advantage of a binary tree is its fast insert and lookup
    // characteristics. This version does not deal with tree balancing.

    // Define tree nodes
    [Serializable]
    public class TTreeNode<K, V>
    {
        public V value;
        public K key;
        public TTreeNode<K, V> left, right;

        // Constructor  to create a single node 
        public TTreeNode(K k, V v)
        {
            key = k;
            value = v;
            left = null;
            right = null;
        }

        public V[] getAllValues()
        {
            List<V> r = new List<V>();
            r.Add(value);
            r.AddRange(left == null ? new V[0] : left.getAllValues());
            r.AddRange(right == null ? new V[0] : right.getAllValues());
            return r.ToArray();
        }

        public V[] getAllValues(ref int readedNondes)
        {
            readedNondes++;
            List<V> r = new List<V>();
            r.Add(value);
            r.AddRange(left == null ? new V[0] : left.getAllValues(ref readedNondes));
            r.AddRange(right == null ? new V[0] : right.getAllValues(ref readedNondes));
            return r.ToArray();
        }

        public K[] getAllKeys()
        {
            List<K> r = new List<K>();
            r.Add(key);
            r.AddRange(left == null ? new K[0] : left.getAllKeys());
            r.AddRange(right == null ? new K[0] : right.getAllKeys());
            return r.ToArray();
        }
    }

    public enum CompareResult
    {
        Equal,
        Less,
        Greater
    }

    // The Binary tree itself
    [Serializable]
    public class TBinarySTree<K, V>
    {
        // Implements:

        // count()
        // clear()
        // insert()
        // delete()
        // findSymbol()
        //
        // Usage:
        //
        //  TBinarySTree bt = new TBinarySTree();
        //  bt.insert ("Bill", "3.14");
        //  bt.insert ("John". 2.71");
        //  etc.
        //  node = bt.findSymbol ("Bill");
        //  WriteLine ("Node value = {0}\n", node.value);
        //

        public delegate CompareResult CompareMethod<K>(K elem1, K elem2);

        public TTreeNode<K, V> root; // Points to the root of the tree
        private int _count = 0;
        private CompareMethod<K> comparator;

        public TBinarySTree(CompareMethod<K> compareMethod)
        {
            comparator = compareMethod;
            root = null;
            _count = 0;
        }

        // Recursive destruction of binary search tree, called by method clear
        // and destroy. Can be used to kill a sub-tree of a larger tree.
        // This is a hanger on from its Delphi origins, it might be dispensable
        // given the garbage collection abilities of .NET
        private void killTree(ref TTreeNode<K, V> p)
        {
            if (p != null)
            {
                killTree(ref p.left);
                killTree(ref p.right);
                p = null;
            }
        }

        /// <summary>
        /// Clear the binary tree.
        /// </summary>
        public void clear()
        {
            killTree(ref root);
            _count = 0;
        }

        /// <summary>
        /// Returns the number of nodes in the tree
        /// </summary>
        /// <returns>Number of nodes in the tree</returns>
        public int count()
        {
            return _count;
        }

        /// <summary>
        /// Find name in tree. Return a reference to the node
        /// if symbol found else return null to indicate failure.
        /// </summary>
        /// <param name="name">Name of node to locate</param>
        /// <returns>Returns null if it fails to find the node, else returns reference to node</returns>
        public TTreeNode<K, V> find(K key)
        {
            TTreeNode<K, V> np = root;
            CompareResult cmp;
            while (np != null)
            {
                cmp = comparator(key, np.key);
                if (cmp == CompareResult.Equal) // found !
                    return np;

                if (cmp == CompareResult.Less)
                    np = np.left;
                else
                    np = np.right;
            }
            return null; // Return null to indicate failure to find name
        }

        // Recursively locates an empty slot in the binary tree and inserts the node
        private void add(TTreeNode<K, V> node, ref TTreeNode<K, V> tree)
        {
            if (tree == null)
                tree = node;
            else
            {
                // If we find a node with the same name then it's 
                // a duplicate and we can't continue
                CompareResult comparison = comparator(node.key, tree.key);
                if (comparison == CompareResult.Equal)
                    throw new Exception();

                if (comparison == CompareResult.Less)
                {
                    add(node, ref tree.left);
                }
                else
                {
                    add(node, ref tree.right);
                }
            }
        }

        /// <summary>
        /// Add a symbol to the tree if it's a new one. Returns reference to the new
        /// node if a new node inserted, else returns null to indicate node already present.
        /// </summary>
        /// <param name="name">Name of node to add to tree</param>
        /// <param name="d">Value of node</param>
        /// <returns> Returns reference to the new node is the node was inserted.
        /// If a duplicate node (same name was located then returns null</returns>
        public TTreeNode<K, V> insert(K key, V value)
        {
            TTreeNode<K, V> node = new TTreeNode<K, V>(key, value);
            try
            {
                if (root == null)
                    root = node;
                else
                    add(node, ref root);
                _count++;
                return node;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // Searches for a node with name key, name. If found it returns a reference
        // to the node and to thenodes parent. Else returns null.
        private TTreeNode<K, V> findParent(K key, ref TTreeNode<K, V> parent)
        {
            TTreeNode<K, V> np = root;
            parent = null;
            CompareResult cmp;
            while (np != null)
            {
                cmp = comparator(key, np.key);
                if (cmp == CompareResult.Equal) // found !
                    return np;

                if (cmp == CompareResult.Less)
                {
                    parent = np;
                    np = np.left;
                }
                else
                {
                    parent = np;
                    np = np.right;
                }
            }
            return null; // Return null to indicate failure to find name
        }

        /// <summary>
        /// Find the next ordinal node starting at node startNode.
        /// Due to the structure of a binary search tree, the
        /// successor node is simply the left most node on the right branch.
        /// </summary>
        /// <param name="startNode">Name key to use for searching</param>
        /// <param name="parent">Returns the parent node if search successful</param>
        /// <returns>Returns a reference to the node if successful, else null</returns>
        public TTreeNode<K, V> findSuccessor(TTreeNode<K, V> startNode, ref TTreeNode<K, V> parent)
        {
            parent = startNode;
            // Look for the left-most node on the right side
            startNode = startNode.right;
            while (startNode.left != null)
            {
                parent = startNode;
                startNode = startNode.left;
            }
            return startNode;
        }

        /// <summary>
        /// Delete a given node. This is the more complex method in the binary search
        /// class. The method considers three senarios, 1) the deleted node has no
        /// children; 2) the deleted node as one child; 3) the deleted node has two
        /// children. Case one and two are relatively simple to handle, the only
        /// unusual considerations are when the node is the root node. Case 3) is
        /// much more complicated. It requires the location of the successor node.
        /// The node to be deleted is then replaced by the sucessor node and the
        /// successor node itself deleted. Throws an exception if the method fails
        /// to locate the node for deletion.
        /// </summary>
        /// <param name="key">Name key of node to delete</param>
        public void delete(K key)
        {
            TTreeNode<K, V> parent = null;
            // First find the node to delete and its parent
            TTreeNode<K, V> nodeToDelete = findParent(key, ref parent);
            if (nodeToDelete == null)
                throw new Exception("Unable to delete node: " + key.ToString()); // can't find node, then say so 

            // Three cases to consider, leaf, one child, two children

            // If it is a simple leaf then just null what the parent is pointing to
            if ((nodeToDelete.left == null) && (nodeToDelete.right == null))
            {
                if (parent == null)
                {
                    root = null;
                    return;
                }

                // find out whether left or right is associated 
                // with the parent and null as appropriate
                if (parent.left == nodeToDelete)
                    parent.left = null;
                else
                    parent.right = null;
                _count--;
                return;
            }

            // One of the children is null, in this case
            // delete the node and move child up
            if (nodeToDelete.left == null)
            {
                // Special case if we're at the root
                if (parent == null)
                {
                    root = nodeToDelete.right;
                    return;
                }

                // Identify the child and point the parent at the child
                if (parent.left == nodeToDelete)
                    parent.right = nodeToDelete.right;
                else
                    parent.left = nodeToDelete.right;
                nodeToDelete = null; // Clean up the deleted node
                _count--;
                return;
            }

            // One of the children is null, in this case
            // delete the node and move child up
            if (nodeToDelete.right == null)
            {
                // Special case if we're at the root			
                if (parent == null)
                {
                    root = nodeToDelete.left;
                    return;
                }

                // Identify the child and point the parent at the child
                if (parent.left == nodeToDelete)
                    parent.left = nodeToDelete.left;
                else
                    parent.right = nodeToDelete.left;
                nodeToDelete = null; // Clean up the deleted node
                _count--;
                return;
            }

            // Both children have nodes, therefore find the successor, 
            // replace deleted node with successor and remove successor
            // The parent argument becomes the parent of the successor
            TTreeNode<K, V> successor = findSuccessor(nodeToDelete, ref parent);
            // Make a copy of the successor node
            TTreeNode<K, V> tmp = new TTreeNode<K, V>(successor.key, successor.value);
            // Find out which side the successor parent is pointing to the
            // successor and remove the successor
            if (parent.left == successor)
                parent.left = null;
            else
                parent.right = null;

            // Copy over the successor values to the deleted node position
            nodeToDelete.value = tmp.value;
            nodeToDelete.key = tmp.key;
            _count--;
        }

        //// Simple 'drawing' routines
        //private string drawNode (TTreeNode<T>node) {
        //    if (node == null)
        //        return "empty";

        //    if ((node.left == null) && (node.right == null))
        //        return node.value.ToString();
        //    if ((node.left != null) && (node.right == null))
        //        return node.value + "(" + drawNode(node.left) + ", _)";

        //    if ((node.right != null) && (node.left == null))
        //        return node.value + "(_, " + drawNode(node.right) + ")";

        //    return node.value + "(" + drawNode(node.left) + ", " + drawNode(node.right) + ")";
        //}


        ///// <summary>
        ///// Return the tree depicted as a simple string, useful for debugging, eg
        ///// 50(40(30(20, 35), 45(44, 46)), 60)
        ///// </summary>
        ///// <returns>Returns the tree</returns>
        //public string drawTree() {
        //    return drawNode (root);
        //}

        public V[] getAllValues()
        {
            if (root == null)
                return new V[0];

            return root.getAllValues();
        }

        public V[] getAllValues(out int readedNondes)
        {
            readedNondes = 0;
            if (root == null)
                return new V[0];

            return root.getAllValues(ref readedNondes);
        }

        public V[] find(CompareType compare, K key, out int readedNondes)
        {
            readedNondes = 0;
            if (root == null)
                return new V[0];
            return find(compare, key, ref readedNondes, root).ToArray();
        }

        private List<V> find(CompareType compare, K key, ref int readedNondes, TTreeNode<K, V> node)
        {
            List<V> ret = new List<V>();
            if (node == null)
                return ret;

            readedNondes++;
            CompareResult compareResult = comparator(node.key, key);

            switch (compare)
            {
                case CompareType.equal:
                case CompareType.like:
                    if (compareResult == CompareResult.Equal)
                        ret.Add(node.value);
                    else if (compareResult == CompareResult.Less)
                        ret.AddRange(find(compare, key, ref readedNondes, node.right));
                    else
                        ret.AddRange(find(compare, key, ref readedNondes, node.left));
                    break;

                case CompareType.notEqual:
                case CompareType.notLike:
                    if (compareResult != CompareResult.Equal)
                        ret.Add(node.value);
                    ret.AddRange(find(compare, key, ref readedNondes, node.left));
                    ret.AddRange(find(compare, key, ref readedNondes, node.right));
                    break;

                case CompareType.greater:
                    if (compareResult == CompareResult.Greater)
                    {
                        ret.Add(node.value);
                        ret.AddRange(find(compare, key, ref readedNondes, node.left));
                        ret.AddRange(find(compare, key, ref readedNondes, node.right));
                    }
                    else
                        ret.AddRange(find(compare, key, ref readedNondes, node.right));
                    break;
                case CompareType.greaterOrEqual:
                    if (compareResult == CompareResult.Equal)
                    {
                        ret.Add(node.value);
                        ret.AddRange(find(compare, key, ref readedNondes, node.right));
                    }
                    else if (compareResult == CompareResult.Greater)
                    {
                        ret.Add(node.value);
                        ret.AddRange(find(compare, key, ref readedNondes, node.left));
                        ret.AddRange(find(compare, key, ref readedNondes, node.right));
                    }
                    else
                        ret.AddRange(find(compare, key, ref readedNondes, node.right));
                    break;
                case CompareType.less:
                    if (compareResult == CompareResult.Less)
                    {
                        ret.Add(node.value);
                        ret.AddRange(find(compare, key, ref readedNondes, node.left));
                        ret.AddRange(find(compare, key, ref readedNondes, node.right));
                    }
                    else
                        ret.AddRange(find(compare, key, ref readedNondes, node.left));
                    break;
                case CompareType.lessOrEqual:
                    if (compareResult == CompareResult.Equal)
                    {
                        ret.Add(node.value);
                        ret.AddRange(find(compare, key, ref readedNondes, node.left));
                    }
                    else if (compareResult == CompareResult.Less)
                    {
                        ret.Add(node.value);
                        ret.AddRange(find(compare, key, ref readedNondes, node.left));
                        ret.AddRange(find(compare, key, ref readedNondes, node.right));
                    }
                    else
                        ret.AddRange(find(compare, key, ref readedNondes, node.left));
                    break;
            }

            return ret;
        }

        public K[] getAllKeys()
        {
            if (root == null)
                return new K[0];

            return root.getAllKeys();
        }
    }
}