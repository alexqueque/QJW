using ClayObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QJW.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            TestClay();
            Console.WriteLine("Hello");
            Console.ReadKey();
        }


        static void TestClay()
        {
            {
                            // 创建一个空的粘土对象
            dynamic clay1 = new Clay();

            // 从现有的对象创建
            var clay2 = Clay.Object(new { });

            // 从 json 字符串创建，可用于第三方 API 对接，非常有用
            var clay3 = Clay.Parse(@"{""foo"":""json"", ""bar"":100, ""nest"":{ ""foobar"":true } }");
            }


            var clay = Clay.Object(new
            {
                Foo = "json",
                Bar = 100,
                Nest = new
                {
                    Foobar = true
                }
            });

            var r1 = clay.Foo; // "json" - string类型
            var r2 = clay.Bar; // 100 - double类型
            var r3 = clay.Nest.Foobar; // true - bool类型
            var r4 = clay["Nest"]["Foobar"]; // 还可以和 Javascript 一样通过索引器获取

            // 新增
            clay.Arr = new string[] { "NOR", "XOR" }; // 添加一个数组
            clay.Obj1 = new City { }; // 新增一个实例对象
            clay.Obj2 = new { Foo = "abc", Bar = 100 }; // 新增一个匿名类

            // 更新
            clay.Foo = "Furion";
            clay["Nest"].Foobar = false;
            clay.Nest["Foobar"] = true;

            // 删除操作
            clay.Delete("Foo"); // 通过 Delete 方法删除
            clay.Arr.Delete(0); // 支持数组 Delete 索引删除
            clay("Bar");    // 支持直接通过对象作为方法删除
            clay.Arr(1);    // 支持数组作为方法删除

            // 判断属性是否存在
            var a = clay.IsDefined("Foo"); // true
            var b = clay.IsDefined("Foooo"); // false
            var c = clay.Foo(); // true
            var d = clay.Foooo(); // false;

            // 遍历数组
            foreach (string item in clay.Arr)
            {
                Console.WriteLine(item); // NOR, XOR
            }

            // 遍历整个对象属性及值，类似 JavaScript 的 for (var p in obj)
            foreach (KeyValuePair<string, dynamic> item in clay)
            {
                Console.WriteLine(item.Key + ":" + item.Value); // Foo:json, Bar: 100, Nest: { "Foobar":true}, Arr:["NOR","XOR"]
            }


            //dynamic clay = new Clay();
            clay.Arr = new string[] { "Furion", "Fur" };

            // 数组转换示例
            var a1 = clay.Arr.Deserialize<string[]>(); // 通过 Deserialize 方法
            var a2 = (string[])clay.Arr;    // 强制转换
            string[] a3 = clay.Arr; // 声明方式

            // 对象转换示例
            clay.City = new City { Id = 1, Name = "中山市" };
            var c1 = clay.City.Deserialize<City>(); // 通过 Deserialize 方法
            var c2 = (City)clay.City;    // 强制转换
            City c3 = clay.City; // 声明方式

            // 返回 object
            var obj = clay.Solidify();

            // 返回 dynamic
            var obj1 = clay.Solidify<dynamic>();

            // 返回其他任意类型
            var obj2 = clay.Solidify<City>();


            var json = clay.ToString(); // "{\"Foo\":\"json\",\"Bar\":100,\"Nest\":{\"Foobar\":true},\"Arr\":[\"NOR\",\"XOR\"]}"

            //Console.WriteLine(clay3);

        }
    }

    internal class City
    {
       public int Id { get; set; }
       public string Name { get; set; }
    }
}
