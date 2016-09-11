﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using i18n.Helpers;

namespace i18n.Domain.Helpers
{
    public class FileEnumerator
    {
        private readonly IEnumerable<string> _blackList;

        public FileEnumerator(IEnumerable<string> blackList)
        {
            _blackList = blackList;
            foreach (string str in blackList)
                DebugHelpers.WriteLine(str);
        }

        public IEnumerable<string> GetFiles(string path)
        {
            Queue<string> queue = new Queue<string>();
            queue.Enqueue(path);
            while (queue.Count > 0)
            {
                path = queue.Dequeue();
                try
                {
                    foreach (string path1 in Directory.EnumerateDirectories(path))
                    {
                        if (!IsBlackListed(path1))
                            queue.Enqueue(path1);
                    }
                }
                catch (Exception ex)
                {
                    DebugHelpers.WriteLine(ex.ToString());
                }
                IEnumerable<string> files = null;
                try
                {
                    files = Directory.EnumerateFiles(path, "*.*", SearchOption.TopDirectoryOnly);
                }
                catch (Exception ex)
                {
                    DebugHelpers.WriteLine(ex.ToString());
                }
                if (files != null)
                {
                    foreach (string path1 in files)
                    {
                        if (!IsBlackListed(path1))
                            yield return path1;
                    }
                }
            }
        }

        private bool IsBlackListed(string path)
        {
            return _blackList.Any(x => path.StartsWith(x, StringComparison.OrdinalIgnoreCase));
        }
    }
}
