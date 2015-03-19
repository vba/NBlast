(function() {
	"use strict";

	var object = {
		extends : function (target, $super) {
			function T() {
				this.constructor = target;
			}
			for (var property in $super) {
				if ($super.hasOwnProperty(property)) {
					target[property] = $super[property];
				}
			}
			T.prototype = $super.prototype;
			target.prototype = new T();
		}
	};
	module.exports = object;
})();