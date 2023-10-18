using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomAprioriAlgorithm
{
    public class apriori
    {
        public List<string> objects=new List<string>();
        public List<string> objectcross(List<string> objs,int Noofelement=1)
        {
            List<string> rules=new List<string>();
            //string rules = "";
            int j = 1;
            for (int i=0;i<objs.Count;i=i+1)
            {
                
                while(j!=objs.Count)
                {
                    
                    //rules += objs[i] +"<X>"+objs[j].ToString();
                    
                    //MessageBox.Show(objs[i] + "<X>" + objs[j].ToString());
                    rules.Add(objs[i] + "<X>" + objs[j].ToString());
                    j = j + 1;
                }
                j = i + 2;

            }
            return rules;
        }
        public List<Tuple<string,string>> multipleobjects(List<string> list1,List<string> list2)
        {
            List<Tuple<string, string>> rules=new List<Tuple<string, string>>();
            bool add = true;
            foreach(string obj in list1)
            {
                for(int i=0;i<list2.Count;i=i+1)
                {
                    if (!obj.Contains(list2[i]))
                    {


                        add = true;
                            foreach(var s in rules)
                            {
                                List<string> strings = obj.Split(new string[] { "<X>" }, StringSplitOptions.None).ToList();
                                strings.Add(list2[i]);
                                if(strings.All(s1=>s.Item1.Contains(s1)))
                            {
                                add= false;
                            }
                            
                            }
                        if(rules.Count==0 || add==true)
                        {
                            rules.Add(Tuple.Create(obj + "<X>" + list2[i],obj));
                            //MessageBox.Show(obj + "<X>" + list2[i]);
                            add = false;
                        }
                        
                    }
                }
            }
            
            return rules;
        }
        public List<string> extractlist(List<string> rules)
        {
            List<string> list = new List<string>();
            foreach (string s in rules)
            {
                string[] strings = s.Split(new string[] { "<X>" }, StringSplitOptions.None);
                foreach(string str in strings)
                {
                    if(!list.Contains(str))
                    {
                        //MessageBox.Show(str);
                        list.Add(str);
                    }
                }
            }
            return list;
        }
        //List<Tuple<string, float, float>>
        public List<string> runaprioribysupport(List<string> transactions,List<string> items,float minimumsupport)
        {
            List<string> rules = new List<string>();
            int minimumsupportcount = ((int)((minimumsupport / 100) * (transactions.Count+1)));
            MessageBox.Show(minimumsupportcount.ToString());
            List<int> counts= new List<int>(new int[items.Count]);
            
            foreach(string t in transactions)
            {
                List<string> strings = t.Split(new string[] { "," }, StringSplitOptions.None).ToList();
                for (int i= 0;i < items.Count;i=i+1)
                {
                    
                    if (strings.Any(s => items[i].Contains(s)))
                    {
                        counts[i] = counts[i] + 1;
                    }
                }
            }
            List<string> itemset= new List<string>();
            for (int i= 0;i<counts.Count;i=i+1)
            {
                if (counts[i]>=minimumsupportcount)
                {
                    itemset.Add(items[i]);
                    rules.Add(items[i]+" , Count=" + counts[i]+" , threshold=notcalculate");
                    //MessageBox.Show(items[i]+" , Count=" + counts[i]+" , threshold=notcalculate");
                }
            }
            List<string> seconditem = objectcross(itemset);
            counts = new List<int>(new int[seconditem.Count]);
            foreach (string t in transactions)
            {
                
                for (int i = 0; i < seconditem.Count; i = i + 1)
                {
                    List<string> strings = seconditem[i].Split(new string[] { "<X>" }, StringSplitOptions.None).ToList();
                    if (strings.All(s => t.Contains(s)))
                    {
                        counts[i] = counts[i] + 1;
                    }
                }
            }
            itemset = new List<string>();
            for (int i = 0; i < counts.Count; i = i + 1)
            {
                if (counts[i] >= minimumsupportcount)
                {
                    itemset.Add(seconditem[i]);
                    rules.Add(seconditem[i] + " , Count=" + counts[i] + " , threshold=notcalculate");
                    //MessageBox.Show(seconditem[i] + " , Count=" + counts[i] + " , threshold=notcalculate");
                }
            }
            List<Tuple<string,string>> getrules = new List<Tuple<string, string>>();
            do
            {
                List<string> extractitems = extractlist(itemset);
                getrules=multipleobjects(itemset, extractitems);
                itemset=new List<string>();
                if(getrules.Count== 0)
                {
                    break;
                }
                counts = new List<int>(new int[getrules.Count]);
                foreach (string t in transactions)
                {
                    for (int i = 0; i < getrules.Count; i = i + 1)
                    {
                        List<string> strings = getrules[i].Item1.Split(new string[] { "<X>" }, StringSplitOptions.None).ToList();
                        if (strings.All(s => t.Contains(s)))
                        {
                            counts[i] = counts[i] + 1;
                        }
                    }
                }
                for (int i = 0; i < counts.Count; i = i + 1)
                {
                    if (counts[i] >= minimumsupportcount)
                    {
                        itemset.Add(getrules[i].Item1);
                        rules.Add(getrules[i].Item1 + " , Count=" + counts[i] + " , threshold=notcalculate");
                        //MessageBox.Show(getrules[i].Item1 + " , Count=" + counts[i] + " , threshold=notcalculate");
                    }
                }
                /*List<int> upcounts = new List<int>(new int[getrules.Count]);
                List<int> downcounts = new List<int>(new int[getrules.Count]);
                foreach (string t in transactions)
                {

                    for (int i = 0; i < getrules.Count; i = i + 1)
                    {
                        List<string> strings = getrules[i].Item1.Split(new string[] { "<X>" }, StringSplitOptions.None).ToList();
                        
                        if (strings.All(s => t.Contains(s)))
                        {
                            upcounts[i] = upcounts[i] + 1;
                        }
                        strings = getrules[i].Item2.Split(new string[] { "<X>" }, StringSplitOptions.None).ToList();

                        if (strings.All(s => t.Contains(s)))
                        {
                            downcounts[i] = downcounts[i] + 1;
                        }
                    }
                }
                foreach (string t in transactions)
                {

                    for (int i = 0; i < itemset.Count; i = i + 1)
                    {
                        List<string> strings = items[i].Split(new string[] { "<X>" }, StringSplitOptions.None).ToList();

                        if (strings.All(s => t.Contains(s)))
                        {
                            downcounts[i] = downcounts[i] + 1;
                        }
                    }
                }
                int k = 0;
                foreach (int s in upcounts)
                {
                    MessageBox.Show(getrules[k].Item1);
                    MessageBox.Show(((s / downcounts[k])*100).ToString());
                    k = k + 1;
                }*/
            }
            while(getrules.Count!=0);
            return rules;


        }
    }
}
