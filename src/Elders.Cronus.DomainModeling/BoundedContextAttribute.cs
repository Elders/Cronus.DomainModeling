using System;
using System.Globalization;
using System.Text;

namespace Elders.Cronus.DomainModeling
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class BoundedContextAttribute : Attribute
    {
        private string boundedContextName;

        private string boundedContextNamespace;

        private string companyName;

        private string productName;

        private string productNamespace;


        /// <summary>
        /// Initializes a new instance of the <see cref="BoundedContextAttribute"/> class.
        /// </summary>
        /// <param name="boundedContextNamespace">The bounded context namespace.</param>
        public BoundedContextAttribute(string boundedContextNamespace)
        {
            this.boundedContextNamespace = boundedContextNamespace;
            string[] splitted = boundedContextNamespace.Split('.');
            this.companyName = splitted[0];
            StringBuilder productNameBuilder = new StringBuilder();  // Replace with regex
            for (int i = 1; i < splitted.Length - 1; i++)
            {
                productNameBuilder.Append(splitted[i]);
                productNameBuilder.Append('.');
            }
            this.productName = productNameBuilder.ToString().TrimEnd('.');
            this.boundedContextName = splitted[splitted.Length - 1];
            this.productNamespace = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", companyName, productName);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="BoundedContextAttribute"/> class.
        /// </summary>
        /// <param name="companyName">Name of the company.</param>
        /// <param name="productName">Name of the product.</param>
        /// <param name="boundedContextName">Name of the bounded context.</param>
        public BoundedContextAttribute(string companyName, string productName, string boundedContextName)
        {
            this.boundedContextName = boundedContextName;
            this.productName = productName;
            this.companyName = companyName;
            this.boundedContextNamespace = String.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}", companyName, productName, boundedContextName);
            this.productNamespace = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", companyName, productName);
        }


        /// <summary>
        /// Gets the name of the bounded context.
        /// </summary>
        /// <value>
        /// The name of the bounded context.
        /// </value>
        public string BoundedContextName { get { return boundedContextName; } }


        /// <summary>
        /// Gets the bounded context namespace.
        /// </summary>
        /// <value>
        /// The bounded context namespace.
        /// </value>
        public string BoundedContextNamespace { get { return boundedContextNamespace; } }


        /// <summary>
        /// Gets the name of the company.
        /// </summary>
        /// <value>
        /// The name of the company.
        /// </value>
        public string CompanyName { get { return companyName; } }


        /// <summary>
        /// Gets the name of the product.
        /// </summary>
        /// <value>
        /// The name of the product.
        /// </value>
        public string ProductName { get { return productName; } }


        /// <summary>
        /// Gets the product namespace.
        /// </summary>
        /// <value>
        /// The product namespace.
        /// </value>
        public string ProductNamespace { get { return productNamespace; } }

    }
}
