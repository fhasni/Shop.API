using System;
namespace Shop.API.Common
{
    public class QueryParameters
    {
        private const int _maxSize = 100;

        private int _size = 50;

        public int Page { get; set; }

        public int Size {

            get { return _size; }

            set { _size = Math.Min(value, _maxSize); }

        }

        public string SortBy { get; set; } = "Id";

        private string _sortOrder { get; set; } = "asc";

        public string SortOrder {

            get { return _sortOrder;  }

            set {

                if (value == "asc" || value == "desc") {
                    _sortOrder = value;
                } else {
                    _sortOrder = "asc";
                }

            }

        }



    }
}
