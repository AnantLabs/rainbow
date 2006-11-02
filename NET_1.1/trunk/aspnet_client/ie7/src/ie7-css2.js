/*
	W3C compliance for Microsoft Internet Explorer

	this module forms part of IE7
	IE7 version 0.7.2 (alpha) 2004/08/22
	by Dean Edwards, 2004
*/
if (window.IE7) IE7.addModule("ie7-css2", function() {
// this has lost its oo shape due to IE5.0 inadequcies and
//  the demands of multiple inheritance (sometimes it's just
//  easier that way).

// constants
var CHILD = />/g, ANCHOR = /(\ba(\.[\w-]+)?)$/i;

// cache ie7 classes
IE7.classes = [];
// i can;t remember why the parser has to be exposed
IE7.parser = new Parser;
// ie7 constructors are stored on the IE7 interface
//  this is in anticipation of ie7-css-strict.js
IE7.Class = Class;
IE7.DynamicStyle = DynamicStyle;
IE7.PseudoElement = PseudoElement;
// replace unknown css2/3 selectors with ie7 classes
IE7.parse = function() {
	// parse the style sheet
	with (this.parser) this.cssText = decode(parse(encode(this.cssText)));
	// execute underlying queries of IE7 classes
	for (var i = 0; i < IE7.classes.length; i++) IE7.classes[i].exec();
	// create pseudo elements
	for (i = 0; i < pseudoElements.length; i++) pseudoElements[i].create();
};

// -----------------------------------------------------------------------
//  parser
// -----------------------------------------------------------------------

// override getCSSText function defined in ie7-core.
getCSSText = function(styleSheet, path) {
	// load from source
	return load(styleSheet.href, path);
};

var encoded = []; // private
function Parser() {
	// public
	this.parse = function(cssText) {
		// create regular expressions
		Class.ALL = new RegExp("[^},\\s]*([>+~][^:@,\\s{]+|:(" + pseudoClasses +
			")|\\.[\\w-]+\\.[\\w-.]+|@[@\\d]+)", "g");
		Class.COMPLEX = new RegExp("[^\\s(]+[+~]|@\\d+|:(link|visited|" + pseudoClasses + "|" +
			dynamicPseudoClasses + ")|\\.[\\w-.]+", "g");
		DynamicStyle.ALL = new RegExp("([^}]*):(" + dynamicPseudoClasses + ")([^{]*)", "g");
		// parse out unknown CSS selectors
		return cssText
		.replace(PseudoElement.ALL, PseudoElement.ID)
		.replace(DynamicStyle.ALL, DynamicStyle.ID)
		.replace(Class.ALL, Class.ID);
	};

	this.encode = function(cssText) {
		// create regular expressions
		AttributeSelector.ALL = new RegExp("\\[([^" + attributeTests + "=\\]]+)([" +
			attributeTests + "]?=?)([^\\]]+)?\\]", "g");

		return cssText
		// parse out attribute selectors
		.replace(AttributeSelector.ALL, AttributeSelector.ID)
		// encode style blocks
		.replace(/\{[^\}]*\}/g, function($){return "{"+(push(encoded,$)-1)+"}"})
		// remove double semi-colons (::before)
		.replace(/::/g, ":")
		// split comma separated selectors
		.replace(/([^\}\s]*\,[^\{]*)(\{\d+\})/g, function(match, left, right) {
			return left.split(",").join(right) + right;
		});
	};

	// put style blocks back
	this.decode = function(cssText) {
		return cssText.replace(/\{(\d+)\}/g, function($, $1){return encoded[$1]});
	};
};

// -----------------------------------------------------------------------
// IE7 style classes
// -----------------------------------------------------------------------

// virtual
function _Class() {
	// properties
//- this.id = 0;
//- this.name = "";
//- this.selector = "";
//- this.MATCH = null;
	this.toString = function() {
		return "." + this.name;
	};
	// methods
	this.add = function(element) {
		// allocate this class
		element.className += " " + this.name;
	};
	this.remove = function(element) {
		// deallocate this class
		element.className = element.className.replace(this.MATCH, "");
	};
	this.exec = function() {
		// execute the underlying css query for this class
		var match = cssQuery(this.selector);
		// add the class name for all matching elements
		for (var i = 0; i < match.length; i++) this.add(match[i]);
	};
};

// constructor
function Class(selector, cssText) {
	this.id = IE7.classes.length;
	this.name = Class.PREFIX + this.id;
	this.selector = selector;
	this.MATCH = new RegExp("\\s" + this.name + "\\b", "g");
	push(IE7.classes, this);
};
// inheritance
Class.ancestor = _Class;
Class.prototype = new _Class;
// constants
Class.PREFIX = "ie7_";

// class methods
Class.ID = function(match) {
	return simpleSelector(match) + new Class(match);
};

// -----------------------------------------------------------------------
// IE7 dynamic style
// -----------------------------------------------------------------------

// virtual
function _DynamicStyle() {
//- this.attach = "";
//- this.dynamicPseudoClass = null;
//- this.target = "";
	// execute the underlying css query for this class
	this.exec = function() {
		var match = cssQuery(this.attach);
		// process results
		for (var i = 0; i < match.length; i++) {
			// retrieve the event handler's target element(s)
			var target = (this.target) ? cssQuery(this.target, match[i]) : [match[i]];
			// attach event handlers for dynamic pseudo-classes
			if (target) this.dynamicPseudoClass(match[i], target, this);
		}
	};
};
// inheritance
_DynamicStyle.prototype = new _Class;

// constructor
function DynamicStyle(selector, attach, dynamicPseudoClass, target) {
	// initialise object properties
	this.attach = attach;
	this.dynamicPseudoClass = dynamicPseudoClasses[dynamicPseudoClass];
	this.target = target;
	// inheritance
	this.inherit = Class;
	this.inherit(selector);
};
// inheritance
DynamicStyle.ancestor = _DynamicStyle;
DynamicStyle.prototype = new _DynamicStyle;
// class methods
DynamicStyle.ID = function(match, attach, dynamicPseudoClass, target) {
	// no need to capture anchor events
	if (isHTML && dynamicPseudoClass != "focus" && ANCHOR.test(attach)) return match;
	return simpleSelector(match) + new DynamicStyle(match, attach, dynamicPseudoClass, target);
};;

// -----------------------------------------------------------------------
// IE7 pseudo elements
// -----------------------------------------------------------------------

HEADER += ".ie7_anon{vertical-align:bottom;display:inline}";

var pseudoElements = [];

// virtual
function _PseudoElement() {
//- this.position = "before";
	this.content = "";
	this.toString = function(){return ""};
	function addTimer(object, content, cssText) {
		var timer = setInterval(function() {
		try {
			if (!object.load) return;
			object.load(object, content, cssText);
			clearInterval(timer);
		} catch (ignore) {
			// remote scripting
			clearInterval(timer);
		}}, 10);
	};
	// execute the underlying css query for this class
	this.create = function() {
		if (!this.content) return;
		// add the class name for all matching elements
		for (var i = 0; i < this.match.length; i++) {
			var parentElement = this.match[i];
			var pseudoElement = parentElement.runtimeStyle[this.position];
			if (pseudoElement) {
				var object = document.createElement(PseudoElement.OBJECT);
				object.className = "ie7_anon";
				if (this.position == "before" && parentElement.firstChild) {
					parentElement.insertBefore(object, parentElement.firstChild);
				} else {
					parentElement.appendChild(object);
				}
				addTimer(object, pseudoElement.content, pseudoElement.cssText);
				parentElement.runtimeStyle[this.position] = null;
			}
		}
	};
	// execute the underlying css query for this class
	this.exec = function() {
		// execute the underlying css query for this class
		this.match = cssQuery(this.selector);
		// add the class name for all matching elements
		for (var i = 0; i < this.match.length; i++) {
			var runtimeStyle = this.match[i].runtimeStyle;
			if (!runtimeStyle[this.position]) runtimeStyle[this.position] = {cssText:""};
			runtimeStyle[this.position].cssText += ";" + this.cssText;
			if (this.content) runtimeStyle[this.position].content = this.content;
		}
	};
};
// inheritance
_PseudoElement.prototype = new _Class;
// constructor
function PseudoElement(selector, position, cssText) {
	// initialise object properties
	this.position = position;
	this.cssText = encoded[cssText].slice(1, -1);
	var content = this.cssText.match(PseudoElement.CONTENT);
	if (content) this.content = unquote(strings[unquote(content[1])] || content[1]);
	// inheritance
	this.inherit = Class;
	this.inherit(selector);
	// store this class so we can execute it later
	push(pseudoElements, this);
};
// inheritance
PseudoElement.ancestor = _PseudoElement;
PseudoElement.prototype = new _PseudoElement;
// class methods
PseudoElement.ID = function(match, selector, position, cssText) {
	return new PseudoElement(selector, position, cssText);
};
PseudoElement.ALL = /([^}]*):(before|after)[^{]*\{([^}]*)\}/g;
PseudoElement.CONTENT = /content\s*:\s*([^;]*)(;|$)/;
PseudoElement.OBJECT = "<object data='" + makePath("ie7-content.htm", path) +
"' width=100% height=0 type=text/x-scriptlet>";

// -----------------------------------------------------------------------
// selectors
// -----------------------------------------------------------------------

selectors[">"] = function(filtered, from, filter, scopeName) {
	for (var i = 0; i < from.length; i++) {
		var subset = from[i].children;
		for (var j = 0; j < subset.length; j++)
			if (compareTagName(subset[j], filter, scopeName)) push(filtered, subset[j]);
	}
};

selectors["+"] = function(filtered, from, filter, scopeName) {
	for (var i = 0; i < from.length; i++) {
		var adjacent = nextElement(from[i]);
		if (adjacent && compareTagName(adjacent, filter, scopeName)) push(filtered, adjacent);
	}
};

selectors["@"] = function(filtered, from, filter) {
	filter = attributeSelectors[filter];
	for (var i = 0; i < from.length; i++) if (filter(from[i])) push(filtered, from[i]);
};

// -----------------------------------------------------------------------
// pseudo-classes
// -----------------------------------------------------------------------

pseudoClasses["first-child"] = function(element) {
	return !previousElement(element);
};

pseudoClasses["lang"] = function(element, filterArgs) {
	filterArgs = new RegExp("^" + filterArgs, "i");
	while (element && !element.getAttribute("lang")) element = element.parentNode;
	return element && filterArgs.test(element.getAttribute("lang"));
};

dynamicPseudoClasses.hover = function(element) {
	var instance = arguments;
	addEventHandler(element, "onmouseover", function() {
		IE7.Event.hover.register(instance);
	});
	addEventHandler(element, "onmouseout", function() {
		IE7.Event.hover.unregister(instance);
	});
};

dynamicPseudoClasses.active = function(element) {
	var instance = arguments;
	addEventHandler(element, "onmousedown", function() {
		IE7.Event.active.register(instance);
	});
};

dynamicPseudoClasses.focus = function(element) {
	var instance = arguments;
	addEventHandler(element, "onfocus", function() {
		IE7.Event.focus.register(instance);
	});
	addEventHandler(element, "onblur", function() {
		IE7.Event.focus.unregister(instance);
	});
	// check focus of the active element
	if (element == document.activeElement) {
		IE7.Event.focus.register(instance)
	}
};

// globally trap the mouseup event (thanks Martijn!)
addEventHandler(document, "onmouseup", function() {
	var ie7Event = IE7.Event.active;
	var instances = ie7Event.instances, i;
	for (i in instances) ie7Event.unregister(instances[i]);
	ie7Event = IE7.Event.hover;
	instances = ie7Event.instances;
	for (i in instances)
		if (!instances[i][0].contains(event.srcElement))
			ie7Event.unregister(instances[i]);
});

// -----------------------------------------------------------------------
//  attribute selectors
// -----------------------------------------------------------------------

var attributeSelectors = [];

function AttributeSelector(attribute, compare, value) {
	// properties
	value = strings[unquote(value)] || value;
	this.id = attributeSelectors.length;
	// build the test expression
	switch (attribute.toLowerCase()) {
		case "id":
			attribute = "element.id.replace(/ms_\\d+/g,'')";
			break;
		case "class":
			attribute = "element.className.replace(/\\b\\s*ie7_\\d+/g,'')";
			break;
		default:
			attribute = "element.getAttribute('" + attribute + "')";
	}
	// continue building the test expression
	compare = attributeTests[compare];
	push(attributeSelectors, new Function("element", "return " + compare(attribute, value)));
};
AttributeSelector.ID = function(match, attribute, compare, value) {
	return new AttributeSelector(attribute, compare, value);
};
AttributeSelector.prototype.toString = function() {
	return AttributeSelector.PREFIX + this.id;
};
attributeTests = {
	toString: function() {
		var toString = [];
		for (var i in this) if (i) push(toString, i);
		return toString.join("").replace(/=/g, "");

	},
	"": function(attribute) {
		return attribute;
	},
	"=": function(attribute, value) {
		return attribute + "==" + quote(value);
	},
	"~=": function(attribute, value) {
		return "/(^|\\s)" + unquote(value) + "(\\s|$)/.test(" + attribute + ")";
	},
	"|=": function(attribute, value) {
		return "/^" + unquote(value) + "(-|$)/.test(" + attribute + ")";
	}
};
// constants
AttributeSelector.PREFIX = "@";

// -----------------------------------------------------------------------
//  IE7 events
// -----------------------------------------------------------------------

// virtual
function _ie7Event() {
	// properties
//- this.type = "";
//- this.instances = null;
	// methods
	this.register = function(instance) {
		var element = instance[0];
		var target = instance[1];
		var Class = instance[2];
		for (var i = 0; i < target.length; i++) Class.add(target[i]);
		this.instances[Class.id + element.uniqueID] = instance;
	};
	this.unregister = function(instance) {
		var element = instance[0];
		var target = instance[1];
		var Class = instance[2];
		for (var i = 0; i < target.length; i++) Class.remove(target[i]);
		delete this.instances[Class.id + element.uniqueID];
	};
};

// constructor
IE7.Event = function(type) {
	this.type = type;
	this.instances = {};
	IE7.Event[type] = this;
};
// inheritance
IE7.Event.prototype = new _ie7Event;

// ie7 events
new IE7.Event("hover");
new IE7.Event("active");
new IE7.Event("focus");

// -----------------------------------------------------------------------
// generic functions
// -----------------------------------------------------------------------

function simpleSelector(selector) {
	// attempt to preserve specificity for "loose" parsing by
	//  removing unknown tokens from a css selector but keep as
	//  much as we can..
	return selector.replace(Class.COMPLEX, "").replace(CHILD, " ");
};

}, true);
