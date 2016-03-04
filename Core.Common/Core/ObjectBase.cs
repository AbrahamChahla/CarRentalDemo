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
    public abstract class ObjectBase : NotificationObject, IDirtyCapable, IExtensibleDataObject, IDataErrorInfo
    {
        protected ObjectBase()
        {
            _Validator = GetValidator();
            Validate();
        }
        #region Properties

        protected IEnumerable<ValidationFailure> _ValidationErrors = null;
        protected IValidator _Validator = null;

        protected bool _IsDirty = false;

        [NotNavigable]
        public virtual bool IsValid
        {
            get
            {
                if (_ValidationErrors != null && _ValidationErrors.Any())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

        }


        public static CompositionContainer Container { get; set; }

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


        string IDataErrorInfo.Error
        {
            get { return string.Empty; }
        }
        string IDataErrorInfo.this[string columName]
        {
            get
            {
                StringBuilder errors = new StringBuilder();
                if (_ValidationErrors != null && _ValidationErrors.Any())
                {
                    foreach (ValidationFailure validationError in _ValidationErrors)
                    {
                        if (validationError.PropertyName == columName)
                        {
                            errors.AppendLine(validationError.ErrorMessage);
                        }
                    }
                }
                return errors.ToString();
            }
        }
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
        #endregion

        #region IExtensibleDataObject Members
        public ExtensionDataObject ExtensionData { get; set; }
        #endregion
        #region Methods
        public virtual IValidator GetValidator()
        {
            return null;
        }


        public void Validate()
        {
            if (_Validator != null)
            {
                ValidationResult results = _Validator.Validate(this);
                _ValidationErrors = results.Errors;
            }
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName, true);
        }
       
        protected virtual void OnPropertyChanged(string propertyName, bool makeDirty)
        {
            OnPropertyChanged(propertyName);
            if (makeDirty)
            {
                IsDirty = true;
            }
            Validate();
        }
        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            string propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
            OnPropertyChanged(propertyName);
            
        }
        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression, bool makeDirty)
        {
            OnPropertyChanged(propertyExpression);

        }

        public List<ObjectBase> GetDirtyObjects()
        {
            List<ObjectBase> dirtyObjects = new List<ObjectBase>();
            WalkObjectGraph(o =>
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

            WalkObjectGraph(o =>
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

            WalkObjectGraph(o =>
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

        /// <summary>
        /// Method that walk throught an object and check if is dirty.
        /// </summary>
        /// <param name="snippetForObject"></param>
        /// <param name="snippetForCollection"></param>
        /// <param name="exemptProperties"></param>
        protected void WalkObjectGraph(Func<ObjectBase, bool> snippetForObject, Action<IList> snippetForCollection, params string[] exemptProperties)
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

        #endregion




    }
}
