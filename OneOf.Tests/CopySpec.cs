using System;
using NUnit.Framework;

namespace OneOf.Tests {

	public interface IJwOf { object Value { get; } }

	public struct JwOf<T0, T1> : IJwOf {

		readonly object value;
		readonly int index;

		JwOf(object value, int index) { this.value = value; this.index = index; }

		object IJwOf.Value { get { return value; } }

		T Get<T>(int index) {
			if (index != this.index) {
				throw new InvalidOperationException("Cannot return as T" + index + " as result is T" + this.index);
			}
			return (T)value;
		}

		public bool IsT0 { get { return index == 0; } }
		public T0 AsT0 { get { return Get<T0>(0); } }
		public static implicit operator JwOf<T0, T1>(T0 t) {
			return new JwOf<T0, T1>(t, 0);
		}


		public bool IsT1 { get { return index == 1; } }
		public T1 AsT1 { get { return Get<T1>(1); } }
		public static implicit operator JwOf<T0, T1>(T1 t) {
			return new JwOf<T0, T1>(t, 1);
		}

		public void Match(Action<T0> f0, Action<T1> f1) {

			if (this.IsT0 && f0 != null) { f0(this.AsT0); return; }
			if (this.IsT1 && f1 != null) { f1(this.AsT1); return; }

			throw new InvalidOperationException();
		}


		public TResult Match<TResult>(Func<T0, TResult> f0, Func<T1, TResult> f1) {

			if (this.IsT0 && f0 != null) return f0(this.AsT0);
			if (this.IsT1 && f1 != null) return f1(this.AsT1);

			throw new InvalidOperationException();
		}


		public TResult MatchSome<TResult>(Func<T0, TResult> f0 = null, Func<T1, TResult> f1 = null, Func<TResult> otherwise = null) {

			if (this.IsT0 && f0 != null) return f0(this.AsT0);
			if (this.IsT1 && f1 != null) return f1(this.AsT1);

			if (otherwise != null) return otherwise();
			throw new InvalidOperationException();
		}



		bool Equals(JwOf<T0, T1> other) {
			return index == other.index && Equals(value, other.value);
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj is OneOf<T0, T1> && Equals(obj);
		}

		public override int GetHashCode() {
			unchecked {
				return ((value != null ? value.GetHashCode() : 0) * 397) ^ index;
			}
		}

	}


	public class CopySpec {

		public JwOf<int, double> GetValue() {
			return 100.0 / 10.0;
		}

		[Test]
		public void ShouldGetValue() {
			var rs = GetValue();
			rs.Match(
				intValue => {
					Assert.Fail();
				},
				doubleValue => { }
			);
		}
	}
}

