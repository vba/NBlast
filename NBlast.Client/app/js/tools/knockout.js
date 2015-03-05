(function () {
	'use strict';

	module.exports = {
		absorbEnter: function (d, e) {
			return e && e.keyCode !== 13;
		}
	};
})();