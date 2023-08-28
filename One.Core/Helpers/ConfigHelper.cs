using One.Core.Attributes;
using One.Core.Models.Configs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace One.Core.Helpers
{
    public static class ConfigHelper
    {
        #region AllConfig

        public static AllConfig ReadAllConfig(string path)
        {
            return IOHelper.Instance.ReadContentFromLocal<AllConfig>(path);
        }

        public static void SaveAllConfig(AllConfig allConfig, string path)
        {
            IOHelper.Instance.WriteContentTolocal(allConfig, path);
        }

        #endregion AllConfig

        #region CommonConfig

        /// <summary> 保存通用配置 </summary>
        /// <typeparam name="T1"> </typeparam>
        /// <param name="commonSetting"> </param>
        /// <returns> </returns>
        public static List<CommonConfigM> SaveCommonConfig<T1>(T1 commonSetting)
        {
            List<CommonConfigM> commonConfigs = new List<CommonConfigM>();

            var temp = typeof(T1);
            var allProperty = temp.GetProperties();

            foreach (var item in allProperty)
            {
                commonConfigs.Add(item.ExportCommonConfig(commonSetting));
            }

            //commonConfigs = commonConfigs.OrderBy(x => x.Name).ToList();

            return commonConfigs;
        }

        /// <summary> 读取通用配置 </summary>
        /// <param name="obj">              </param>
        /// <param name="CommonConfigList"> </param>
        public static void GetCommonConfig(object obj, List<CommonConfigM> CommonConfigList)
        {
            try
            {
                var type = obj.GetType();
                var allProperty = type.GetProperties();
                foreach (var propertyInner in allProperty)
                {
                    foreach (var itemInner in CommonConfigList)
                    {
                        if ((propertyInner.Name == itemInner.Name))
                        {
                            if (propertyInner.PropertyType.IsClass && propertyInner.PropertyType.FullName != "System.String")
                            {
                                Type typeInner = propertyInner.PropertyType;
                                object typeValue = propertyInner.GetValue(obj);
                                GetCommonConfig(typeValue, itemInner.CommonConfigList);
                            }
                            else
                            {
                                ParseObjectValue(propertyInner, obj, itemInner.Value);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary> 导出通用配置（递归） </summary>
        /// <param name="prop"> </param>
        /// <param name="obj">  </param>
        /// <returns> </returns>
        private static CommonConfigM ExportCommonConfig(this PropertyInfo prop, object obj)
        {
            CommonConfigM config = new CommonConfigM();
            config.Name = prop.Name;

            var attributes = (DescriptionAttribute[])prop.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length == 0)
            {
                config.Description = "";
            }
            else
            {
                config.Description = attributes[0].Description;
            }

            config.Type = prop.PropertyType.FullName;

            if (prop.PropertyType.IsEnum)
            {
                var type = prop.PropertyType;
                var value = prop.GetValue(obj).ToString();
                var aa = (int)Enum.Parse(type, value);
                config.Value = aa;
            }
            else if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
            {
                var type = prop.PropertyType;
                var allProperty = type.GetProperties();

                foreach (var item in allProperty)
                {
                    var current = prop.GetValue(obj);
                    //var value = item.GetValue(current);
                    var temp = item.ExportCommonConfig(current);
                    config.CommonConfigList.Add(temp);
                }
            }
            else
            {
                if (prop.PropertyType == typeof(short) ||
                    prop.PropertyType == typeof(ushort) ||
                     prop.PropertyType == typeof(int) ||
                      prop.PropertyType == typeof(uint) ||
                       prop.PropertyType == typeof(long) ||
                       prop.PropertyType == typeof(ulong))
                {
                    if (double.TryParse(prop.GetValue(obj).ToString(), out double aa))
                    {
                        config.Value = aa;
                    }
                    else
                    {
                        config.Value = prop.GetValue(obj);
                    }
                }
                else
                {
                    config.Value = prop.GetValue(obj);
                }
            }

            return config;
        }

        #endregion CommonConfig

        #region TestItemConfig

        /// <summary> 导出测试项配置 </summary>
        /// <typeparam name="T"> 测试项父类 </typeparam>
        /// <typeparam name="T2"> 测试项配置父类 </typeparam>
        /// <param name="testItemSetting"> </param>
        /// <returns> </returns>
        public static List<TestSecquenceConfigM> SaveTestSecquenceConfig<T, T2>(T2 testItemSetting)
        {
            List<TestSecquenceConfigM> testSecquenceConfigs = new List<TestSecquenceConfigM>();

            var temp1 = typeof(T);
            var allMethods = temp1.GetMethods();
            foreach (var item in allMethods)
            {
                var attributes = (ExportAttribute[])item.GetCustomAttributes(typeof(ExportAttribute), false);

                if (attributes.Length > 0)
                {
                    var export = attributes[0].Export;
                    if (export)
                    {
                        var attributeCus = (CustomerAttribute[])item.GetCustomAttributes(typeof(CustomerAttribute), false);
                        var attributeDec = (DescriptionAttribute[])item.GetCustomAttributes(typeof(DescriptionAttribute), false);

                        TestSecquenceConfigM testSecquenceConfig = new TestSecquenceConfigM();
                        testSecquenceConfig.TestItem = item.Name;
                        //客户类型
                        if (attributeCus.Length > 0)
                        {
                            testSecquenceConfig.Customer = attributeCus[0].Customer;
                        }
                        else
                        {
                            testSecquenceConfig.Customer = "";
                        }
                        //描述
                        if (attributeDec.Length > 0)
                        {
                            testSecquenceConfig.Description = attributeDec[0].Description;
                        }

                        #region 测试项默认设置

                        var temp2 = typeof(T2);
                        var allProperty = temp2.GetProperties();
                        foreach (var item2 in allProperty)
                        {
                            var CustomerSettingAttributes = (CustomerSettingAttribute[])item2.GetCustomAttributes(typeof(CustomerSettingAttribute), false);
                            var item2Value = item2.GetValue(testItemSetting);
                            if (CustomerSettingAttributes.Length > 0)
                            {
                                var CustomerSettingAttribute = CustomerSettingAttributes[0].TargetTestItemName;
                                if (CustomerSettingAttribute == testSecquenceConfig.TestItem)
                                {
                                    var temp3 = item2.PropertyType;

                                    foreach (var temp3property in temp3.GetProperties())
                                    {
                                        //var temp3propertyValue = temp3property.GetValue(item2Value);
                                        testSecquenceConfig.TestSecquenceItemConfigList.Add(temp3property.ExportCommonConfig(item2Value));
                                    }
                                }
                            }
                        }

                        #endregion 测试项默认设置

                        testSecquenceConfigs.Add(testSecquenceConfig);
                    }
                }
            }
            testSecquenceConfigs = testSecquenceConfigs.OrderBy(x => x.TestItem).ToList();//.Substring(0, 1)

            return testSecquenceConfigs;
        }

        /// <summary> 读取测试项配置 </summary>
        /// <param name="obj">              </param>
        /// <param name="CommonConfigList"> </param>
        public static void GetTestItemConfig(object obj, List<TestSecquenceConfigM> CommonConfigList)
        {
            try
            {
                var type = obj.GetType();
                var allProperty = type.GetProperties();
                foreach (var property in allProperty)
                {
                    if (!property.PropertyType.IsClass)
                    {
                        throw new Exception("Format Error,Not Class!");
                    }

                    var CustomerSettingAttributes = (CustomerSettingAttribute[])property.GetCustomAttributes(typeof(CustomerSettingAttribute), false);
                    //带标头的类的实例
                    var propertyValue = property.GetValue(obj);
                    if (CustomerSettingAttributes.Length > 0)
                    {
                        //待填充属性的特性标头
                        var CustomerSettingAttribute = CustomerSettingAttributes[0].TargetTestItemName;

                        foreach (var testSecquenceConfig in CommonConfigList)
                        {
                            //找到父节点的类和对应的测试项
                            if (CustomerSettingAttribute == testSecquenceConfig.TestItem)
                            {
                                //找到子节点的类的属性和对应的测试项下边的参数列表
                                foreach (var propertyInner in property.PropertyType.GetProperties())
                                {
                                    foreach (var itemInner in testSecquenceConfig.TestSecquenceItemConfigList)
                                    {
                                        if (propertyInner.PropertyType.IsClass && propertyInner.PropertyType.FullName != "System.String")
                                        {
                                            throw new Exception("Format Error,Is Class!");
                                        }
                                        else
                                        {
                                            if (propertyInner.Name == itemInner.Name)
                                            {
                                                ParseObjectValue(propertyInner, propertyValue, itemInner.Value);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion TestItemConfig

        #region Update

        /// <summary> 更新通用测试项内容 </summary>
        /// <param name="old">      </param>
        /// <param name="newModel"> </param>
        /// <returns> </returns>
        public static List<CommonConfigM> UpdateCommonConfig(List<CommonConfigM> old, List<CommonConfigM> newModel)
        {
            List<CommonConfigM> newCommonConfigList = new List<CommonConfigM>();
            foreach (var newConfig in newModel)
            {
                var temp = newConfig.Clone() as CommonConfigM;
                foreach (var oldItem in old)
                {
                    //找到父节点的类和对应的测试项
                    if (oldItem.Name == newConfig.Name)
                    {
                        temp.Value = oldItem.Value;
                        var tempList = UpdateCommonConfig(oldItem.CommonConfigList, newConfig.CommonConfigList);
                        temp.CommonConfigList = tempList;

                        break;
                    }
                }

                newCommonConfigList.Add(temp);
            }
            return newCommonConfigList;
        }

        /// <summary> 更新已有的测试序列 </summary>
        /// <param name="TestSecquences">           </param>
        /// <param name="AllSupportTestSecquences"> </param>
        /// <returns> </returns>
        public static List<TestSecquenceConfigM> UpdateTestSecquenceConfig(List<TestSecquenceConfigM> TestSecquences, List<TestSecquenceConfigM> AllSupportTestSecquences)
        {
            List<TestSecquenceConfigM> newTestSecquences = new List<TestSecquenceConfigM>();

            foreach (var oldTestSecquenceConfig in TestSecquences)
            {
                foreach (var AlltestSecquenceConfig in AllSupportTestSecquences)
                {
                    //找到父节点的类和对应的测试项
                    if (oldTestSecquenceConfig.TestItem == AlltestSecquenceConfig.TestItem)
                    {
                        var temp = AlltestSecquenceConfig.Clone() as TestSecquenceConfigM;
                        temp.Index = oldTestSecquenceConfig.Index;

                        temp.TestSecquenceItemConfigList = UpdateCommonConfig(oldTestSecquenceConfig.TestSecquenceItemConfigList, AlltestSecquenceConfig.TestSecquenceItemConfigList);

                        newTestSecquences.Add(temp);
                        break;
                    }
                }
            }
            return newTestSecquences;
        }

        #endregion Update

        #region Help

        private static void ParseObjectValue(PropertyInfo propertyInner, object target, object value)
        {
            if (propertyInner.PropertyType.IsEnum)
            {
                var typeEnum = propertyInner.PropertyType;
                if (value is string)
                {
                    string stringValue = value.ToString();
                    bool isInt = int.TryParse(stringValue, out int intValue);

                    if (isInt)
                    {
                        propertyInner.SetValue(target, Enum.ToObject(typeEnum, intValue));
                    }
                }
                else
                {
                    var aa = Enum.GetName(typeEnum, value);
                    propertyInner.SetValue(target, Enum.Parse(typeEnum, aa));
                }
            }
            else if (propertyInner.PropertyType.FullName == "System.Boolean")
            {
                Boolean intValue = Convert.ToBoolean(value);
                propertyInner.SetValue(target, intValue);
            }
            else if (propertyInner.PropertyType.FullName == "System.Int32")
            {
                int intValue = Convert.ToInt32(value);
                propertyInner.SetValue(target, intValue);
            }
            else if (propertyInner.PropertyType.FullName == "System.UInt32")
            {
                uint intValue = Convert.ToUInt32(value);
                propertyInner.SetValue(target, intValue);
            }
            else if (propertyInner.PropertyType.FullName == "System.Double")
            {
                double intValue = Convert.ToDouble(value);
                propertyInner.SetValue(target, intValue);
            }
            else if (propertyInner.PropertyType.FullName == "System.String")
            {
                string intValue = Convert.ToString(value);
                propertyInner.SetValue(target, intValue);
            }
            else
            {
                propertyInner.SetValue(target, value);
            }
        }

        #endregion Help
    }
}