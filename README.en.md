 [English](README.en.md) | [简体中文](README.md)
 
![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.5%2B-blue)
![.NET Core](https://img.shields.io/badge/.NET%20Core-3.1%2B-green)
![.NET 5+](https://img.shields.io/badge/.NET%205%2B-Windows%20Desktop-purple)
![Platform](https://img.shields.io/badge/Platform-WPF-orange)
![License](https://img.shields.io/badge/license-MIT-lightgrey)

# WPF TreeView Control Library Usage Guide

## 🔗 Repository Addresses

* NuGet: [Su.WPF.TreeViewEx](https://www.nuget.org/packages/Su.WPF.TreeViewEx/)
* Gitee: [https://gitee.com/SususuChang/su.-wpf.-custom-control](https://gitee.com/SususuChang/su.-wpf.-custom-control)
* GitHub: [https://github.com/ViewSuSu/Su.WPF.TreeViewEx](https://github.com/ViewSuSu/Su.WPF.TreeViewEx)

## Overview

**The world has suffered from TreeView long enough!** The internet is filled with various fragmented solutions, from complex custom templates to cumbersome data binding. Developers have been racking their brains to implement a fully functional TreeView control in WPF. However, to this day, there is no unified, complete, and easy-to-use encapsulation solution.

**Now I present a viable solution!**

This project provides a complete, object-oriented tree node operation approach, freeing developers from tedious template definitions, data binding, and event handling, allowing them to focus on business logic implementation.

## 📦 NuGet Package Installation

### Install via Package Manager Console

```powershell
Install-Package Su.WPF.TreeViewEx
```

### Install via .NET CLI

```bash
dotnet add package Su.WPF.TreeViewEx
```

### Install via Visual Studio Package Manager

1. Right-click project → **Manage NuGet Packages**
2. Search for `Su.WPF.TreeViewEx`
3. Click **Install**

### Package Reference (csproj)

```xml
<PackageReference Include="Su.WPF.TreeViewEx" Version="1.0.0" />
```

## 🎬 Demo Animation

<div align="left">
<img src="./HD.gif" alt="TreeViewEx Feature Demo" width="30%">
</div>

### 🌟 Zero-Dependency Pure Implementation
**Important Feature: No Third-Party Dependencies!**
```csharp
// Pure native WPF implementation
// ✅ No dependency on MVVM frameworks (Prism, MVVMLight, etc.)
// ✅ No dependency on UI component libraries (MaterialDesign, MahApps, etc.)  
// ✅ No dependency on IOC containers (Autofac, Unity, etc.)
// ✅ No dependency on any other NuGet packages

// Ready to use out of the box, no complex dependency configuration and environment setup required
```

### 🔒 Secure API Design

```csharp
// Compiler prevents incorrect usage
// ❌ These will cause compilation errors, avoiding runtime exceptions:
// node.MenuItems.Add(...);        // MenuItems is read-only
// provider.Controller = null;     // Controller is read-only
// node.Children = new List<>();   // Children collection is protected

// ✅ Can only operate through designed public methods
node.AddChild("Secure Operation");
node.MenuItemModels.Add(menu);     // Correct collection operation
```

## 🚧 Current Version Status

**Optimization work continues!** The current version has reasonably implemented core requirements and provides stable and usable basic functionality, but the project recognizes there is still room for improvement:

### ✅ Implemented Stable Features

* **Complete tree structure management** - Basic operations like node creation, deletion, traversal
* **Checkbox system** - Three-state management, automatic cascading, status query
* **Right-click menu system** - Tree-level and node-level menus, with shortcut key support
* **Style configuration system** - Visual customization of icons, colors, fonts, etc.
* **Selection management** - Multi-select support, selection status tracking
* **Copy functionality** - Complete node structure copying
* **Data binding** - Association with custom data objects
* **Tree structure multi-select** - Supports Shift/Ctrl/Ctrl+A for multi-selecting tree nodes

### ⚠️ Known Areas for Optimization

#### 1. **Performance Optimization Needs**

* **Virtualized display**: The current version may have performance bottlenecks when handling large-scale node data (e.g., 1000+ nodes). Plans to introduce virtualization technology to only render nodes in the visible area, significantly improving performance.
* **Memory management**: Further optimization needed for node object lifecycle management to reduce memory usage.

#### 2. **Asynchronous Operation Support**

* **Data loading**: All current operations are synchronous, which may block the UI thread when processing large amounts of data or remote data.
* **Batch operations**: Need to implement asynchronous batch node operations for better user experience.

#### 3. **Advanced Feature Planning**

* **Lazy loading**: Support for lazy loading of child nodes, loading data only when needed.
* **Animation effects**: Smooth animation effects for node expansion/collapse.
* **Drag and drop support**: Complete drag and drop operation support.
* **Filtering and search**: Real-time filtering and search functionality.

## 🚀 Quick Start

### 1. After installing NuGet package, configure in XAML

```xml
<Grid>
    <ContentControl Content="{Binding Provider.TreeView, Mode=OneWay}" />
</Grid>
```

### 2. ViewModel Basic Structure

```csharp
using Su.WPF.CustomControl.TreeViewEx;
using Su.WPF.CustomControl.Menu;

public class MainWindowViewModel
{
    public TreeViewExProvider Provider { get; }
    public List<TreeNodeEx> TreeNodeExs { get; set; }

    public MainWindowViewModel()
    {
        // 1. Create tree node structure
        InitializeTreeNodes();
        
        // 2. Get tree view provider
        Provider = TreeViewExProvider.GetTreeViewPanelProvider(TreeNodeExs);
        
        // 3. Configure menu system
        ConfigureMenus();
    }

    private void InitializeTreeNodes()
    {
        // Create node structure
        TreeNodeExs = new List<TreeNodeEx>
        {
            TreeNodeEx.CreateNode("Root Node")
        };
    }

    private void ConfigureMenus()
    {
        // Configure tree-level and node-level menus
    }
}
```

## 🎨 Core Feature Usage

### 1. **Basic Tree Structure Creation**

```csharp
// Create root node
var root = TreeNodeEx.CreateNode("My Project");

// Add child nodes
var srcFolder = root.AddChild("Source Code");
srcFolder.AddChild("MainWindow.xaml.cs");
srcFolder.AddChild("MainViewModel.cs");

// Batch add
root.AddRange(new[] {
    TreeNodeEx.CreateNode("Documentation"),
    TreeNodeEx.CreateNode("Resources")
});
```

### 2. **Icon Configuration**

```csharp
var node = TreeNodeEx.CreateNode("Node with Icon");

// Set icon and size
node.TreeNodeExIconOptions.Icon = yourImageSource;
node.TreeNodeExIconOptions.Width = 20;
```

### 3. **Text Style Configuration**

```csharp
var titleNode = TreeNodeEx.CreateNode("Title");
titleNode.TreeNodeExTextOptions.FontSize = 16;
titleNode.TreeNodeExTextOptions.FontWeight = FontWeights.Bold;
```

### 4. **Checkbox System**

```csharp
var parent = TreeNodeEx.CreateNode("Parent Node");
parent.IsShowCheckBox = true;

var child = parent.AddChild("Child Node");
child.IsShowCheckBox = true;
child.IsChecked = true;
```

### 5. **Menu System and Shortcut Keys**

#### Tree-Level Menus (with Shortcut Key Support)

```csharp
// Basic shortcut
var refreshMenu = new TreeViewMenu("Refresh", RefreshAction)
{
    Shortcut = new MenuShortcut(ModifierKeys.Control, Key.R)
};

// Combined shortcut
var saveMenu = new TreeViewMenu("Save", SaveAction)
{
    Shortcut = new MenuShortcut(ModifierKeys.Control | ModifierKeys.Shift, Key.S)
};

provider.Controller.Options.MenuItemModels.Add(refreshMenu);
provider.Controller.Options.MenuItemModels.Add(saveMenu);
```

#### Node-Level Menus

```csharp
var fileNode = TreeNodeEx.CreateNode("File");
fileNode.MenuItemModels.Add(
    new TreeNodeMenu("Open", node => OpenFile(node))
);
```

### 6. **Advanced Query Functions**

```csharp
// Copy node
var copiedNode = originalNode.Copy();

// Checkbox status query
var checkedNodes = parent.GetCheckedChildren();
var allCheckedDescendants = parent.GetAllCheckedDescendants();
var hasCheckedChildren = parent.HasCheckedChildren();
```

## 📋 API Reference

### Node Operations

| Method | Description |
|------|------|
| `TreeNodeEx.CreateNode(text)` | Create new node |
| `node.AddChild(text)` | Add text child node |
| `node.AddChild(childNode)` | Add child node object |
| `node.AddRange(nodes)` | Batch add child nodes |
| `node.Copy()` | Copy node and its subtree |
| `node.CopyTo(parent)` | Copy to specified parent node |
| `node.Delete()` | Delete node |

### Checkbox Queries

| Method | Description |
|------|------|
| `GetCheckedChildren()` | Get checked nodes among direct children |
| `GetAllCheckedDescendants()` | Get all checked nodes among descendants |
| `GetCheckBoxChildrenCount()` | Get count of child nodes showing checkboxes |
| `GetCheckedChildrenCount()` | Get count of checked child nodes |
| `HasCheckedChildren()` | Check if there are any checked child nodes |

### Menus and Shortcut Keys

| Property/Method | Description |
|-----------|------|
| `Controller.Options.MenuItemModels` | Tree-level menu collection |
| `node.MenuItemModels` | Node-level menu collection |
| `new TreeViewMenu(header, action)` | Create tree-level menu |
| `new TreeNodeMenu(header, action)` | Create node-level menu |
| `menu.Shortcut` | Set menu shortcut key |
| `menu.ShortcutDisplay` | Get shortcut key display text |
| `new MenuShortcut(modifiers, key)` | Create shortcut key configuration |

## 🎯 Applicable Scenarios

### Current Version Applicable Scenarios

* ✅ Small to medium scale data (node count < 500)
* ✅ Local data operations
* ✅ Synchronous data processing
* ✅ Basic tree structure display
* ✅ Need complete right-click menu support
* ✅ Need checkbox functionality
* ✅ Need shortcut key support
* ✅ Need icon and style customization

### Scenarios for Optimized Version

* 🔄 Large scale data (node count > 1000)
* 🔄 Remote data loading
* 🔄 Asynchronous operation requirements
* 🔄 High-performance requirement scenarios
* 🔄 Complex animation effect requirements

## 🎯 Core Design Philosophy

### 🏗️ Robust Architecture Based on SOLID Principles

This project adopts **object-oriented design thinking**, strictly following **SOLID principles**, especially the **Open/Closed Principle**:

* **✅ Open for extension**: You can easily inherit base classes like `TreeNodeEx`, `MenuBase`, etc., to add custom functionality
* **✅ Closed for modification**: Core APIs are stable, version updates won't break existing code
* **✅ Single responsibility**: Each class has clear responsibility boundaries, avoiding "god objects"
* **✅ Interface segregation**: Provides fine-grained configuration options, use as needed

## Summary

**While optimization continues, the current version is already powerful enough!**

This project provides a **stable, feature-complete, easy-to-use** TreeView solution that solves 90% of daily development needs. For those scenarios requiring extreme performance with ultra-large-scale data, the project is actively developing optimized versions.

## 💡 Design Philosophy

**"Simple shouldn't be complex, complex shouldn't be simple"**

* Common functionality should be **ready to use out of the box**
* Advanced functionality should be **extensible**
* Incorrect usage should result in **compile-time errors**
* Architecture design should be **future-oriented**

---
## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

**If this project is helpful to you, please give it a ⭐ Star!**


## 星曲线图 / Star History

[![Star History Chart](https://api.star-history.com/svg?repos=ViewSuSu/Su.WPF.TreeViewEx&type=Date)](https://star-history.com/#ViewSuSu/Su.WPF.TreeViewEx&Date)
