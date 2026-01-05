 [English](README.en.md) | [中文](README.md)
![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.5%2B-blue)
![.NET Core](https://img.shields.io/badge/.NET%20Core-3.1%2B-green)
![.NET 5+](https://img.shields.io/badge/.NET%205%2B-Windows%20Desktop-purple)
![Platform](https://img.shields.io/badge/Platform-WPF-orange)
![License](https://img.shields.io/badge/license-MIT-lightgrey)

# WPF 树控件TreeView封装库使用说明

## 🔗 仓库地址

* NuGet: [Su.WPF.TreeViewEx](https://www.nuget.org/packages/Su.WPF.TreeViewEx/)
* Gitee：[https://gitee.com/SususuChang/su.-wpf.-custom-control](https://gitee.com/SususuChang/su.-wpf.-custom-control)
* GitHub：[https://github.com/ViewSuSu/Su.WPF.TreeViewEx](https://github.com/ViewSuSu/Su.WPF.TreeViewEx)

## 概述

**天下苦TreeView久矣！** 网上充斥着各种五花八门的解决方案，从复杂的自定义模板到繁琐的数据绑定，开发者们为了在WPF中实现一个功能完整的TreeView控件可谓是绞尽脑汁。然而，至今仍没有一个统一的、完整的、易于使用的封装方案。

**现在我给出一种可行的方案！**

该项目提供了一套完整的、面向对象的树节点操作方式，让开发者从繁琐的模板定义、数据绑定、事件处理中解放出来，专注于业务逻辑的实现。

## 📦 NuGet 包安装

### 通过包管理器控制台安装

```powershell
Install-Package Su.WPF.TreeViewEx
```

### 通过 .NET CLI 安装

```bash
dotnet add package Su.WPF.TreeViewEx
```

### 通过 Visual Studio 包管理器安装

1. 右键点击项目 → **管理 NuGet 程序包**
2. 搜索 `Su.WPF.TreeViewEx`
3. 点击 **安装**

### 包引用 (csproj)

```xml
<PackageReference Include="Su.WPF.TreeViewEx" Version="1.0.0" />
```

## 🎬 演示动画

<div align="left">
<img src="./HD.gif" alt="TreeViewEx 功能演示" width="30%">
</div>

### 🌟 零依赖的纯净实现
**重要特性：无任何第三方依赖！**
```csharp
// 纯原生 WPF 实现
// ✅ 不依赖 MVVM 框架（Prism、MVVMLight等）
// ✅ 不依赖 UI 组件库（MaterialDesign、MahApps等）  
// ✅ 不依赖 IOC 容器（Autofac、Unity等）
// ✅ 不依赖任何其他 NuGet 包

// 开箱即用，无需复杂的依赖配置和环境搭建
```

### 🔒 安全的API设计

```csharp
// 编译器会阻止错误的使用方式
// ❌ 这些都会编译错误，避免运行时异常：
// node.MenuItems.Add(...);        // MenuItems是只读的
// provider.Controller = null;     // Controller是只读的
// node.Children = new List<>();   // Children集合保护

// ✅ 只能通过设计好的公共方法操作
node.AddChild("安全操作");
node.MenuItemModels.Add(menu);     // 正确的集合操作
```

## 🚧 当前版本状态

**优化工作仍在继续！** 现版本已经合理实现了核心需求，提供了稳定可用的基础功能，但该项目深知还有提升空间：

### ✅ 已实现的稳定功能

* **完整的树形结构管理** - 节点创建、删除、遍历等基础操作
* **复选框系统** - 三态管理、自动级联、状态查询
* **右键菜单系统** - 树级别和节点级别菜单，支持快捷键
* **样式配置系统** - 图标、颜色、字体等视觉定制
* **选择管理** - 多选支持、选择状态跟踪
* **复制功能** - 完整节点结构复制
* **数据绑定** - 关联自定义数据对象
* **树形结构多选** - 支持Shitf/Ctrl/Ctrl+A对树节点进行多选

### ⚠️ 已知待优化项

#### 1. **性能优化需求**

* **虚拟化显示**：当前版本在处理大规模节点数据时（如1000+节点），可能存在性能瓶颈。计划引入虚拟化技术，只渲染可见区域的节点，大幅提升性能。
* **内存管理**：需要进一步优化节点对象的生命周期管理，减少内存占用。

#### 2. **异步操作支持**

* **数据加载**：当前所有操作都是同步的，在处理大量数据或远程数据时可能阻塞UI线程。
* **批量操作**：需要实现异步的批量节点操作，提供更好的用户体验。

#### 3. **高级功能规划**

* **延迟加载**：支持子节点的延迟加载，只在需要时加载数据。
* **动画效果**：节点展开/折叠的平滑动画效果。
* **拖拽支持**：完整的拖拽操作支持。
* **过滤搜索**：实时过滤和搜索功能。

## 🚀 快速开始

### 1. 安装 NuGet 包后，在 XAML 中配置

```xml
<Grid>
    <ContentControl Content="{Binding Provider.TreeView, Mode=OneWay}" />
</Grid>
```

### 2. ViewModel 基础结构

```csharp
using Su.WPF.CustomControl.TreeViewEx;
using Su.WPF.CustomControl.Menu;

public class MainWindowViewModel
{
    public TreeViewExProvider Provider { get; }
    public List<TreeNodeEx> TreeNodeExs { get; set; }

    public MainWindowViewModel()
    {
        // 1. 创建树节点结构
        InitializeTreeNodes();
        
        // 2. 获取树视图提供者
        Provider = TreeViewExProvider.GetTreeViewPanelProvider(TreeNodeExs);
        
        // 3. 配置菜单系统
        ConfigureMenus();
    }

    private void InitializeTreeNodes()
    {
        // 创建节点结构
        TreeNodeExs = new List<TreeNodeEx>
        {
            TreeNodeEx.CreateNode("根节点")
        };
    }

    private void ConfigureMenus()
    {
        // 配置树级别和节点级别菜单
    }
}
```

## 🎨 核心功能使用

### 1. **基本树结构创建**

```csharp
// 创建根节点
var root = TreeNodeEx.CreateNode("我的项目");

// 添加子节点
var srcFolder = root.AddChild("源代码");
srcFolder.AddChild("MainWindow.xaml.cs");
srcFolder.AddChild("MainViewModel.cs");

// 批量添加
root.AddRange(new[] {
    TreeNodeEx.CreateNode("文档"),
    TreeNodeEx.CreateNode("资源")
});
```

### 2. **图标配置**

```csharp
var node = TreeNodeEx.CreateNode("带图标的节点");

// 设置图标和大小
node.TreeNodeExIconOptions.Icon = yourImageSource;
node.TreeNodeExIconOptions.Width = 20;
```

### 3. **文本样式配置**

```csharp
var titleNode = TreeNodeEx.CreateNode("标题");
titleNode.TreeNodeExTextOptions.FontSize = 16;
titleNode.TreeNodeExTextOptions.FontWeight = FontWeights.Bold;
```

### 4. **复选框系统**

```csharp
var parent = TreeNodeEx.CreateNode("父节点");
parent.IsShowCheckBox = true;

var child = parent.AddChild("子节点");
child.IsShowCheckBox = true;
child.IsChecked = true;
```

### 5. **菜单系统与快捷键**

#### 树级别菜单（支持快捷键）

```csharp
// 基本快捷键
var refreshMenu = new TreeViewMenu("刷新", RefreshAction)
{
    Shortcut = new MenuShortcut(ModifierKeys.Control, Key.R)
};

// 组合快捷键
var saveMenu = new TreeViewMenu("保存", SaveAction)
{
    Shortcut = new MenuShortcut(ModifierKeys.Control | ModifierKeys.Shift, Key.S)
};

provider.Controller.Options.MenuItemModels.Add(refreshMenu);
provider.Controller.Options.MenuItemModels.Add(saveMenu);
```

#### 节点级别菜单

```csharp
var fileNode = TreeNodeEx.CreateNode("文件");
fileNode.MenuItemModels.Add(
    new TreeNodeMenu("打开", node => OpenFile(node))
);
```

### 6. **高级查询功能**

```csharp
// 复制节点
var copiedNode = originalNode.Copy();

// 复选框状态查询
var checkedNodes = parent.GetCheckedChildren();
var allCheckedDescendants = parent.GetAllCheckedDescendants();
var hasCheckedChildren = parent.HasCheckedChildren();
```

## 📋 API 参考

### 节点操作

| 方法 | 描述 |
|------|------|
| `TreeNodeEx.CreateNode(text)` | 创建新节点 |
| `node.AddChild(text)` | 添加文本子节点 |
| `node.AddChild(childNode)` | 添加子节点对象 |
| `node.AddRange(nodes)` | 批量添加子节点 |
| `node.Copy()` | 复制节点及其子树 |
| `node.CopyTo(parent)` | 复制到指定父节点 |
| `node.Delete()` | 删除节点 |

### 复选框查询

| 方法 | 描述 |
|------|------|
| `GetCheckedChildren()` | 获取直接子节点中选中的节点 |
| `GetAllCheckedDescendants()` | 获取所有子孙节点中选中的节点 |
| `GetCheckBoxChildrenCount()` | 获取显示复选框的子节点数量 |
| `GetCheckedChildrenCount()` | 获取选中的子节点数量 |
| `HasCheckedChildren()` | 检查是否有选中的子节点 |

### 菜单和快捷键

| 属性/方法 | 描述 |
|-----------|------|
| `Controller.Options.MenuItemModels` | 树级别菜单集合 |
| `node.MenuItemModels` | 节点级别菜单集合 |
| `new TreeViewMenu(header, action)` | 创建树级别菜单 |
| `new TreeNodeMenu(header, action)` | 创建节点级别菜单 |
| `menu.Shortcut` | 设置菜单快捷键 |
| `menu.ShortcutDisplay` | 获取快捷键显示文本 |
| `new MenuShortcut(modifiers, key)` | 创建快捷键配置 |

## 🎯 适用场景

### 当前版本适用场景

* ✅ 中小规模数据（节点数量 < 500）
* ✅ 本地数据操作
* ✅ 同步数据处理
* ✅ 基础树形结构展示
* ✅ 需要完整右键菜单支持
* ✅ 需要复选框功能
* ✅ 需要快捷键支持
* ✅ 需要图标和样式定制

### 待优化版本适用场景

* 🔄 大规模数据（节点数量 > 1000）
* 🔄 远程数据加载
* 🔄 异步操作需求
* 🔄 高性能要求场景
* 🔄 复杂动画效果需求

## 🎯 核心设计理念

### 🏗️ 基于 SOLID 原则的健壮架构

本项目采用**面向对象设计思想**，严格遵循 **SOLID 原则**，特别是**开闭原则**：

* **✅ 对扩展开放**：你可以轻松继承 `TreeNodeEx`、`MenuBase` 等基类，添加自定义功能
* **✅ 对修改关闭**：核心API稳定，版本更新不会破坏现有代码
* **✅ 单一职责**：每个类都有明确的职责边界，避免"上帝对象"
* **✅ 接口隔离**：提供细粒度的配置选项，按需使用

## 总结

**虽然优化仍在继续，但当前版本已经足够强大！**

该项目提供了一个**稳定、功能完整、易于使用**的TreeView解决方案，解决了90%的日常开发需求。对于那些需要极致性能的超大规模数据场景，该项目正在积极开发优化版本。

## 💡 设计哲学

**"简单不应该复杂，复杂不应该简单"**

* 常用功能应该**开箱即用**
* 高级功能应该**可扩展**
* 错误使用应该**编译时报错**
* 架构设计应该**面向未来**

---
## 📄 许可证

本项目采用 MIT 许可证 - 查看 [LICENSE](LICENSE) 文件了解详情。

**如果这个项目对您有帮助，请给个 ⭐ Star 支持一下！**
