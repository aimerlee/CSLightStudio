﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    public class RegHelper_TypeFunction : ICLS_TypeFunction
    {
        Type type;
        public RegHelper_TypeFunction(Type type)
        {
            this.type = type;
        }
        public virtual CLS_Content.Value New(CLS_Content environment, IList<CLS_Content.Value> _params)
        {

            List<Type> types = new List<Type>();
            List<object> objparams = new List<object>();
            foreach (var p in _params)
            {
                types.Add(p.type);
                objparams.Add(p.value);
            }
            CLS_Content.Value value = new CLS_Content.Value();
            value.type = type;
            var con = this.type.GetConstructor(types.ToArray());
            value.value = con.Invoke(objparams.ToArray());
            return value;
        }
        public virtual CLS_Content.Value StaticCall(CLS_Content environment, string function, IList<CLS_Content.Value> _params)
        {

            List<object> _oparams = new List<object>();
            List<Type> types = new List<Type>();
            foreach (var p in _params)
            {
                _oparams.Add(p.value);
                types.Add(p.type);
            }
            var targetop = type.GetMethod(function, types.ToArray());
            CLS_Content.Value v = new CLS_Content.Value();
            v.value = targetop.Invoke(null, _oparams.ToArray());
            v.type = targetop.ReturnType;
            return v;


        }

        public virtual CLS_Content.Value StaticValueGet(CLS_Content environment, string valuename)
        {
            var targetp = type.GetProperty(valuename);
            if (targetp != null)
            {
                CLS_Content.Value v = new CLS_Content.Value();
                v.value = targetp.GetValue(null, null);
                v.type = targetp.PropertyType;
                return v;
            }
            else
            {
                var targetf = type.GetField(valuename);
                if (targetf != null)
                {
                    CLS_Content.Value v = new CLS_Content.Value();
                    v.value = targetf.GetValue(null);
                    v.type = targetf.FieldType;
                    return v;
                }
                else
                {
                    var targete = type.GetEvent(valuename);
                    if(targete!=null)
                    {
                        CLS_Content.Value v = new CLS_Content.Value();

                        v.value = new DeleObject(null, targete);
                        v.type = targete.EventHandlerType;
                        return v;
                    }
                }
            }

            return null;
        }

        public virtual void StaticValueSet(CLS_Content content, string valuename, object value)
        {

            var targetp = type.GetProperty(valuename);
            if (targetp != null)
            {
                if (value != null && value.GetType() != targetp.PropertyType)
                {
                    value = content.environment.GetType(value.GetType()).ConvertTo(content, value, targetp.PropertyType);
                }
                targetp.SetValue(null, value, null);
                return;
            }
            else
            {
                var targetf = type.GetField(valuename);
                if (targetf != null)
                {
                    if (value != null && value.GetType() != targetf.FieldType)
                    {
                        value = content.environment.GetType(value.GetType()).ConvertTo(content, value, targetf.FieldType);
                    }
                    targetf.SetValue(null, value);
                    return;
                }
            }



            throw new NotImplementedException();
        }

        public virtual CLS_Content.Value MemberCall(CLS_Content environment, object object_this, string func, IList<CLS_Content.Value> _params)
        {

            List<Type> types = new List<Type>();
            List<object> _oparams = new List<object>();
            foreach (var p in _params)
            {
                if (p.value is DeleObject)
                {
                    _oparams.Add((p.value as DeleObject).deleInstance);
                }
                else
                {
                    _oparams.Add(p.value);
                }
                types.Add(p.type);
            }

            var targetop = type.GetMethod(func, types.ToArray());
            CLS_Content.Value v = new CLS_Content.Value();
            if (targetop == null)
            {
                throw new Exception("函数不存在function:" + type.ToString() + "." + func                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            );
            }
            v.value = targetop.Invoke(object_this, _oparams.ToArray());
            v.type = targetop.ReturnType;
            return v;
        }

        public virtual CLS_Content.Value MemberValueGet(CLS_Content environment, object object_this, string valuename)
        {
            var targetp = type.GetProperty(valuename);
            if (targetp != null)
            {
                CLS_Content.Value v = new CLS_Content.Value();
                v.value = targetp.GetValue(object_this, null);
                v.type = targetp.PropertyType;
                return v;
            }
            else
            {
                var targetf = type.GetField(valuename);
                if (targetf != null)
                {
                    CLS_Content.Value v = new CLS_Content.Value();
                    v.value = targetf.GetValue(object_this);
                    v.type = targetf.FieldType;
                    return v;
                }
                else
                {
                    System.Reflection.EventInfo targete = type.GetEvent(valuename);
                    if (targete != null)
                    {
                        CLS_Content.Value v = new CLS_Content.Value();
                        v.value =  new DeleObject(object_this, targete);
                        v.type = targete.EventHandlerType;
                        return v;
                    }
                }
            }
               
            return null;
        }

        public virtual void MemberValueSet(CLS_Content content, object object_this, string valuename, object value)
        {

            var targetp = type.GetProperty(valuename);
            if (targetp != null)
            {
                if (value != null && value.GetType() != targetp.PropertyType)
                {
                    value = content.environment.GetType(value.GetType()).ConvertTo(content, value, targetp.PropertyType);
                }

                targetp.SetValue(object_this, value, null);
                return;
            }
            else
            {
                var targetf = type.GetField(valuename);
                if (targetf != null)
                {
                    if (value != null && value.GetType() != targetf.FieldType)
                    {

                        value = content.environment.GetType(value.GetType()).ConvertTo(content, value, targetf.FieldType);
                    }
                    targetf.SetValue(object_this, value);
                    return;
                }
            }



            throw new NotImplementedException();
        }




        public virtual CLS_Content.Value IndexGet(CLS_Content environment, object object_this, object key)
        {
            //var m =type.GetMembers();
            var targetop = type.GetMethod("get_Item");
            if(targetop==null)
            {
                targetop = type.GetMethod("Get");
            }
            
            CLS_Content.Value v = new CLS_Content.Value();
            v.type = targetop.ReturnType;
            v.value = targetop.Invoke(object_this, new object[] { key });
            return v;

        }

        public virtual void IndexSet(CLS_Content environment, object object_this, object key, object value)
        {
            var targetop = type.GetMethod("set_Item");
            targetop.Invoke(object_this, new object[] { key, value });
        }
    }

    public class RegHelper_Type : ICLS_Type
    {
        public RegHelper_Type(Type type, string setkeyword = null)
        {
            function = new RegHelper_TypeFunction(type);
            if (setkeyword != null)
            {
                keyword = setkeyword;
            }
            else
            {
                keyword = type.Name;
            }
            this.type = type;
            this._type = type;
        }

        public string keyword
        {
            get;
            protected set;
        }
        public string _namespace
        {
            get { return type.NameSpace; }
        }
        public CLType type
        {
            get;
            protected set;
        }
        public Type _type;

        public virtual ICLS_Value MakeValue(object value) //这个方法可能存在AOT陷阱
        {
            //这个方法可能存在AOT陷阱
            //Type target = typeof(CLS_Value_Value<>).MakeGenericType(new Type[] { type }); 
            //return target.GetConstructor(new Type[] { }).Invoke(new object[0]) as ICLS_Value;

            CLS_Value_Object rvalue = new CLS_Value_Object(type);
            rvalue.value_value = value;
            return rvalue;
        }

        public virtual object ConvertTo(CLS_Content env, object src, CLType targetType)
        {

            //type.get
            //var m =type.GetMembers();
            var ms = _type.GetMethods();
            foreach (var m in ms)
            {
                if ((m.Name == "op_Implicit" || m.Name == "op_Explicit") && m.ReturnType == (Type)targetType)
                {
                    return m.Invoke(null, new object[] { src });
                }
            }

            return src;
        }

        public virtual object Math2Value(CLS_Content env, char code, object left, CLS_Content.Value right, out CLType returntype)
        {
            returntype = type;
            System.Reflection.MethodInfo call = null;
            //var m = type.GetMethods();
            if (code == '+')
                call = _type.GetMethod("op_Addition", new Type[] { this.type, right.type });
            else if (code == '-')//base = {CLScriptExt.Vector3 op_Subtraction(CLScriptExt.Vector3, CLScriptExt.Vector3)}
                call = _type.GetMethod("op_Subtraction", new Type[] { this.type, right.type });
            else if (code == '*')//[2] = {CLScriptExt.Vector3 op_Multiply(CLScriptExt.Vector3, CLScriptExt.Vector3)}
                call = _type.GetMethod("op_Multiply", new Type[] { this.type, right.type });
            else if (code == '/')//[3] = {CLScriptExt.Vector3 op_Division(CLScriptExt.Vector3, CLScriptExt.Vector3)}
                call = _type.GetMethod("op_Division", new Type[] { this.type, right.type });
            else if (code == '%')//[4] = {CLScriptExt.Vector3 op_Modulus(CLScriptExt.Vector3, CLScriptExt.Vector3)}
                call = _type.GetMethod("op_Modulus", new Type[] { this.type, right.type });

            
            var obj = call.Invoke(null, new object[] { left, right.value });
            //function.StaticCall(env,"op_Addtion",new List<ICL>{})
            return obj;
        }

        public virtual bool MathLogic(CLS_Content env, logictoken code, object left, CLS_Content.Value right)
        {
            System.Reflection.MethodInfo call = null;
            
            //var m = _type.GetMethods();
            if (code == logictoken.more)//[2] = {Boolean op_GreaterThan(CLScriptExt.Vector3, CLScriptExt.Vector3)}
                call = _type.GetMethod("op_GreaterThan");
            else if (code == logictoken.less)//[4] = {Boolean op_LessThan(CLScriptExt.Vector3, CLScriptExt.Vector3)}
                call = _type.GetMethod("op_LessThan");
            else if (code == logictoken.more_equal)//[3] = {Boolean op_GreaterThanOrEqual(CLScriptExt.Vector3, CLScriptExt.Vector3)}
                call = _type.GetMethod("op_GreaterThanOrEqual");
            else if (code == logictoken.less_equal)//[5] = {Boolean op_LessThanOrEqual(CLScriptExt.Vector3, CLScriptExt.Vector3)}
                call = _type.GetMethod("op_LessThanOrEqual");
            else if (code == logictoken.equal)//[6] = {Boolean op_Equality(CLScriptExt.Vector3, CLScriptExt.Vector3)}
            {
                if(left==null || right.type==null)
                {
                    return left == right.value;
                }
                call = _type.GetMethod("op_Equality");
                if(call==null)
                {
                    return type.Equals(right.value);
                }
            }
            else if (code == logictoken.not_equal)//[7] = {Boolean op_Inequality(CLScriptExt.Vector3, CLScriptExt.Vector3)}
            {
                if (left == null || right.type == null)
                {
                    return left != right.value;
                }
                call = _type.GetMethod("op_Inequality");
                if(call==null)
                {
                    return !type.Equals(right.value);
                }
            }
            var obj = call.Invoke(null, new object[] { left, right.value });
            return (bool)obj;
        }

        public ICLS_TypeFunction function
        {
            get;
            protected set;
        }


        public virtual object DefValue
        {
            get { return null; }
        }
    }
}
