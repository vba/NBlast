(function() {
	"use strict";

	var object = {
		'extends' : function (target, $super) {
			for (var property in $super) {
				if ($super.hasOwnProperty(property)) {
					target[property] = $super[property];
				}
			}
			function T() { this.constructor = target; }
			T.prototype = $super.prototype;
			target.prototype = new T();
		}
	};
	module.exports = object;
})();