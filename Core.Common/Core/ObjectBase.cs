using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Core.Common.Contracts;
using Core.Common.Extensions;
using Core.Common.Utils;
using FluentValidation;
using FluentValidation.Results;

namespace Core.Common.Core
{
    public abstract class ObjectBase : NotificationObject, IDirtyCapable, IExtensibleDataObject
    {
        public ObjectBase()
        {
            _Validator = GetValidator();
            Validate();
        }

        protected bool _IsDirty = false;
        protected IValidator _Validator = null;

        protected IEnumerable<ValidationFailure> _ValidationErrors = null;
        public static CompositionContainer Container { get; set; }

        #region IExtensibleDataObject Members
        public ExtensionDataObject ExtensionData { get; set; }
        #endregion

        protected virtual void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName, true);
        }
        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            string propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
            OnPropertyChanged(propertyName);
        }
        protected virtual void OnPropertyChanged(string propertyName, bool makeDirty)
        {
           // base.OnPropertyChanged(propertyName);
            if (makeDirty)
            {
                IsDirty = true;
            }
            Validate();
        }

        public virtual IValidator GetValidator()
        {
            return null;
        }
        [NotNavigable]
        public IEnumerable<ValidationFailure> ValidationErrors
        {
            get
            {
                return _ValidationErrors;
            }
            set
            {

            }
        }
        public void Validate()
        {
            if (_Validator != null)
            {
                ValidationResult results = _Validator.Validate(this);
                _ValidationErrors = results.Errors;
            }
        }
        [NotNavigable]
        public virtual bool IsValid
        {
            get
            {
                if (_ValidationErrors != null && _ValidationErrors.Count() > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

        }

        //string IDataErrorInfo.Error
        //{
        //    get
        //    {
        //        return string.Empty;
        //    }
        //}
        //string IDataErrorInfo.this[string columName]
        //{
        //    get
        //    {
        //        StringBuilder errors = new StringBuilder();
        //        if (_ValidationErrors != null && _ValidationErrors.Count() > 0)
        //        {
        //            foreach (ValidationFailure validationError in _ValidationErrors)
        //            {
        //                if (validationError.PropertyName == columName)
        //                {
        //                    errors.AppendLine(validationError.ErrorMessage);
        //                }
        //            }
        //        }
        //        return errors.ToString();
        //    }
        //}

        [NotNavigable]
        public bool IsDirty
        {
            get
            {
                return _IsDirty;
            }
            set
            {
                _IsDirty = value;
            }
        }
        public List<ObjectBase> GetDirtyObjects()
        {
            List<ObjectBase> dirtyObjects = new List<ObjectBase>();
            WalObjectGraph(o =>
            {
                if (o.IsDirty)
                {
                    dirtyObjects.Add(o);
                }
                return false;
            }, coll => { });

            return dirtyObjects;
        }
        public void CleanAll()
        {

            WalObjectGraph(o =>
            {
                if (o.IsDirty)
                {
                    o.IsDirty = false;
                }
                return false;
            }, coll => { });
        }
        public virtual bool IsAnythingDirty()
        {
            bool IsDirty = false;

            WalObjectGraph(o =>
            {
                if (o.IsDirty)
                {
                    IsDirty = true;
                    return true;
                }
                else
                {
                    return false;

                }
            }, coll => { });
            return IsDirty;
        }
        protected void WalObjectGraph(Func<ObjectBase, bool> snippetForObject, Action<IList> snippetForCollection, params string[] exemptProperties)
        {
            List<ObjectBase> visited = new List<ObjectBase>();
            Action<ObjectBase> walk = null;
            List<string> exemptions = new List<string>();
            if (exemptProperties != null)
            {
                exemptions = exemptProperties.ToList();
            }

            walk = (o) =>
            {
                if (o != null && !visited.Contains(o))
                {
                    visited.Add(o);

                    bool exitWalk = snippetForObject.Invoke(o);
                    if (!exitWalk)
                    {
                        PropertyInfo[] properties = o.GetBrowsableProperties();
                        foreach (var property in properties)
                        {
                            if (!exemptions.Contains(property.Name))
                            {
                                if (property.PropertyType.IsSubclassOf(typeof(ObjectBase)))
                                {
                                    ObjectBase obj = (ObjectBase)(property.GetValue(o, null));
                                    walk(obj);
                                }
                                else
                                {
                                    IList coll = property.GetValue(o, null) as IList;
                                    if (coll != null)
                                    {
                                        snippetForCollection.Invoke(coll);
                                        foreach (var item in coll)
                                        {
                                            if (item is ObjectBase)
                                            {
                                                walk((ObjectBase)item);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            };
            walk(this);

        }





    }
}
