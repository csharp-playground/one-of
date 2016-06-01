using System;
using NUnit.Framework;

namespace OneOf.Tests {

	public class Product {
		public int Id { set; get; }
	}

	public class NotFound {

	}

	public class MatchSpec {

		public OneOf<Product, NotFound> FindProduct(int productId) {
			if (productId == 0) {
				return new NotFound();
			}
			return new Product { Id = productId };
		}

		[Test]
		public void ShouldQueryProduct() {
			var id = 1;
			var rs = FindProduct(id);

			rs.Match(
				pro => {
					Assert.AreEqual(1, pro.Id);
				},
				no => {
					Assert.Fail();
				}
			);
		}
	}
}
