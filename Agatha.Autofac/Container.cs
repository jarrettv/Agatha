using System;
using Agatha.Common.InversionOfControl;
using Autofac;

namespace Agatha.Autofac
{
	public class Container : Agatha.Common.InversionOfControl.IContainer
	{
        private readonly global::Autofac.ContainerBuilder builder;
        private global::Autofac.IContainer container;

        protected global::Autofac.IContainer Container
        {
            get
            {
                if (container == null)
                {
                    container = builder.Build();
                }
                return container;
            }
        }

        public Container() : this(new ContainerBuilder()) { }

		public Container(ContainerBuilder builder)
		{
            this.builder = builder;
		}

		public void Register(Type componentType, Type implementationType, Lifestyle lifeStyle)
		{
            var reg = builder.RegisterType(implementationType)
                .As(componentType);

            if (lifeStyle == Lifestyle.Singleton) reg.SingleInstance();
            else reg.InstancePerLifetimeScope();
		}

        public void Register<TComponent, TImplementation>(Lifestyle lifestyle) where TImplementation : TComponent
		{
			Register(typeof(TComponent), typeof(TImplementation), lifestyle);
		}

		public void RegisterInstance(Type componentType, object instance)
		{
            builder.RegisterInstance(instance).As(componentType);
		}

		public void RegisterInstance<TComponent>(TComponent instance)
		{
			RegisterInstance(typeof(TComponent), instance);
		}

		public TComponent Resolve<TComponent>()
		{
            return this.Container.Resolve<TComponent>();
		}

        public TComponent Resolve<TComponent>(string key)
        {
            return this.Container.ResolveKeyed<TComponent>(key);
        }

		public object Resolve(Type componentType)
		{
            return this.Container.Resolve(componentType);
		}

	    public TComponent TryResolve<TComponent>()
	    {
            TComponent @out = default(TComponent);
            this.Container.TryResolve<TComponent>(out @out);
            return @out;
	    }

	    public void Release(object component)
		{

		}
	}
}