#ifndef __MAPPING_PARSER_H__
#define __MAPPING_PARSER_H__

#include <map>
#include <string>
#include <vector>

namespace OuyaEverywhere
{
	class MappingParser
	{
	public:
		class Button
		{
		public:
			int mSourceKeyCode;
			int mDestinationKeyCode;
			std::vector<int> mExcludeSource;
		};
		class AxisRemap
		{
		public:
			int mSourceAxis;
			int mDestinationAxis;
		};
		class ButtonIsAxis
		{
		public:
			int mSourceAxis;
			float mActionDown;
			int mDestinationKeyCode;
		};
		class Controller
		{
		public:
			std::vector<std::string> mAlias;
			std::vector<AxisRemap> mAxisRemap;
			std::map<int, Button> mButton;
			std::vector<ButtonIsAxis> mButtonIsAxis;
		};
		class Device
		{
		public:
			std::vector<std::string> mAlias;
			std::map<std::string, Controller> mController;
		};
		Button getButton(std::string deviceName, std::string controllerName, int keyCode);
		std::vector<AxisRemap> getAxisRemap(std::string deviceName, std::string controllerName);
		std::vector<ButtonIsAxis> getButtonIsAxis(std::string deviceName, std::string controllerName);
		void parse(std::string jsonData);
	private:
		std::map<std::string, Device> mDevice;
		Device getDevice(std::string deviceName);
		Controller getController(Device device, std::string controllerName);
		int getKeyCode(const std::string& name);
	};
}

#endif