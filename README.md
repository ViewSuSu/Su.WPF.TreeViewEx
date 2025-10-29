
# WPF 树控件封装库使用说明

## 概述

**天下苦TreeView久矣！** 网上充斥着各种五花八门的解决方案，从复杂的自定义模板到繁琐的数据绑定，开发者们为了在WPF中实现一个功能完整的TreeView控件可谓是绞尽脑汁。然而，至今仍没有一个统一的、完整的、易于使用的封装方案。

**现在我给出一种可行的方案！**

该项目提供了一套完整的、面向对象的树节点操作方式，让开发者从繁琐的模板定义、数据绑定、事件处理中解放出来，专注于业务逻辑的实现。

## 🎬 演示动画
<div align="center"> <img src="./HD.gif" alt="TreeViewEx 功能演示" width="50%"> </div>


## 🎯 核心设计理念

### 🏗️ 基于 SOLID 原则的健壮架构

本项目采用**面向对象设计思想**，严格遵循 **SOLID 原则**，特别是**开闭原则**：

- **✅ 对扩展开放**：你可以轻松继承 `TreeNodeEx`、`MenuBase` 等基类，添加自定义功能
- **✅ 对修改关闭**：核心API稳定，版本更新不会破坏现有代码
- **✅ 单一职责**：每个类都有明确的职责边界，避免"上帝对象"
- **✅ 接口隔离**：提供细粒度的配置选项，按需使用

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
- **完整的树形结构管理** - 节点创建、删除、遍历等基础操作
- **复选框系统** - 三态管理、自动级联、状态查询
- **右键菜单系统** - 树级别和节点级别菜单，支持快捷键
- **样式配置系统** - 图标、颜色、字体等视觉定制
- **选择管理** - 多选支持、选择状态跟踪
- **复制功能** - 完整节点结构复制
- **数据绑定** - 关联自定义数据对象

### ⚠️ 已知待优化项

#### 1. **性能优化需求**
- **虚拟化显示**：当前版本在处理大规模节点数据时（如1000+节点），可能存在性能瓶颈。计划引入虚拟化技术，只渲染可见区域的节点，大幅提升性能。
- **内存管理**：需要进一步优化节点对象的生命周期管理，减少内存占用。

#### 2. **异步操作支持**
- **数据加载**：当前所有操作都是同步的，在处理大量数据或远程数据时可能阻塞UI线程。
- **批量操作**：需要实现异步的批量节点操作，提供更好的用户体验。

#### 3. **高级功能规划**
- **延迟加载**：支持子节点的延迟加载，只在需要时加载数据。
- **动画效果**：节点展开/折叠的平滑动画效果。
- **拖拽支持**：完整的拖拽操作支持。
- **过滤搜索**：实时过滤和搜索功能。

## 快速开始

### 1. 基本使用 - 告别复杂配置！

```csharp
// 传统方式：需要定义模板、绑定、样式...
// 该项目的方式：3行代码搞定！
var nodes = new List<TreeNodeEx>
{
    TreeNodeEx.CreateNode("根节点1"),
    TreeNodeEx.CreateNode("根节点2")
};
var provider = TreeViewExProvider.GetTreeViewPanelProvider(nodes);
// <ContentControl Content="{Binding Provider.TreeView, Mode=OneWay}" />
```

## 🎨 完整的样式配置系统

### 1. **图标配置 (TreeNodeExIconOptions)**

```csharp
// 完整的图标配置系统
var node = TreeNodeEx.CreateNode("带图标的节点");

// 设置图标
node.TreeNodeExIconOptions.Icon = BitmapFrame.Create(new Uri("pack://application:,,,/icon.png"));
// 或者从资源中加载
node.TreeNodeExIconOptions.Icon = (ImageSource)FindResource("MyIcon");

// 自定义图标大小
node.TreeNodeExIconOptions.Width = 24;  // 默认16，可调整

// 自动显示逻辑：Icon不为null且Width>0时显示图标
if (node.TreeNodeExIconOptions.IsShowImageSource)
{
    // 图标会显示在节点文本前
}
```

**图标配置属性说明：**
- `Icon: ImageSource` - 图标图像源，支持所有WPF图像格式
- `Width: double` - 图标显示宽度，高度按比例自动调整
- `IsShowImageSource: bool` - 只读属性，判断是否满足显示条件

### 2. **文本样式配置 (TreeNodeExTextOptions)**

```csharp
// 完整的文本样式配置
var node = TreeNodeEx.CreateNode("样式化文本");

// 字体大小配置
node.TreeNodeExTextOptions.FontSize = 14;  // 默认系统字体大小

// 字体粗细配置
node.TreeNodeExTextOptions.FontWeight = FontWeights.Bold;      // 加粗
node.TreeNodeExTextOptions.FontWeight = FontWeights.SemiBold;  // 半粗
node.TreeNodeExTextOptions.FontWeight = FontWeights.Normal;    // 正常（默认）

// 实际应用场景
var titleNode = TreeNodeEx.CreateNode("章节标题");
titleNode.TreeNodeExTextOptions.FontSize = 16;
titleNode.TreeNodeExTextOptions.FontWeight = FontWeights.Bold;

var contentNode = TreeNodeEx.CreateNode("内容文本");  
contentNode.TreeNodeExTextOptions.FontSize = 12;
contentNode.TreeNodeExTextOptions.FontWeight = FontWeights.Normal;
```

**文本样式属性说明：**
- `FontSize: double` - 字体大小，默认使用系统消息字体大小
- `FontWeight: FontWeight` - 字体粗细，默认正常粗细

### 3. **高亮颜色配置**

```csharp
// 节点高亮颜色配置
node.HighlightColor = Colors.Blue;                    // 使用系统颜色
node.HighlightColor = Color.FromRgb(255, 0, 0);       // RGB颜色
node.HighlightColor = Color.FromArgb(255, 0, 120, 215); // 主题色

// 自动生成画刷属性
Brush highlightBrush = node.HighlightColorBrush;  // 只读，自动创建
```

### 2. 完整的树结构构建 - 像操作普通对象一样简单！

```csharp
// 传统方式：需要定义复杂的数据模型和绑定
// 该项目的方式：直观的对象操作
var project = TreeNodeEx.CreateNode("我的项目");
project.TreeNodeExIconOptions.Icon = LoadIcon("project.png");
project.TreeNodeExIconOptions.Width = 20; // 自定义图标大小

var srcFolder = project.AddChild("源代码");
srcFolder.TreeNodeExIconOptions.Icon = folderIcon;
srcFolder.AddChild("MainWindow.xaml.cs");
srcFolder.AddChild("MainViewModel.cs");

var configFolder = project.AddChild("配置");
configFolder.AddChild("app.config");

// 添加右键菜单 - 使用 MenuItemModels！
srcFolder.MenuItemModels.Add(new TreeNodeMenu("新建文件", node => {
    node.AddChild($"新文件_{DateTime.Now:HHmmss}.cs");
}));
```

### 3. 复选框功能 - 自动级联，零配置！

```csharp
// 传统方式：需要手动处理复选框状态同步
// 该项目的方式：开箱即用的自动级联
var parent = TreeNodeEx.CreateNode("父节点");
parent.IsShowCheckBox = true;

var child1 = parent.AddChild("子节点1");
child1.IsShowCheckBox = true;
child1.IsChecked = true;

var child2 = parent.AddChild("子节点2"); 
child2.IsShowCheckBox = true;
child2.IsChecked = false;

// 自动效果：
// - 选中父节点 → 所有子节点自动选中
// - 取消父节点 → 所有子节点自动取消  
// - 部分子节点选中 → 父节点显示为部分选中状态
// 所有这些都不需要你写一行状态同步代码！
```

### 4. 🆕 完整的菜单系统 - 支持快捷键和图标！

```csharp
// 树级别菜单（整个树控件共享）
provider.Controller.Options.MenuItemModels.Add(new TreeViewMenu("刷新", RefreshTree)
{
    Shortcut = new MenuShortcut(ModifierKeys.Control, Key.R),
    Icon = refreshIcon
});

// 节点级别菜单（特定节点独有）
var fileNode = TreeNodeEx.CreateNode("重要文件.txt");
fileNode.MenuItemModels.Add(new TreeNodeMenu("加密", node => EncryptFile(node)));
fileNode.MenuItemModels.Add(new TreeNodeMenu("备份", node => BackupFile(node)));

// 带快捷键的菜单项（自动注册全局快捷键）
var copyMenu = new TreeViewMenu("复制", CopyAction)
{
    Shortcut = new MenuShortcut(ModifierKeys.Control, Key.C)
};
// 自动显示为："复制 (Ctrl+C)" 并响应 Ctrl+C 快捷键

// 带图标的菜单项
var saveMenu = new TreeViewMenu("保存", SaveAction)
{
    Icon = saveIcon
};
```

### 5. 🆕 快捷键系统 - 自动注册和管理！

```csharp
// 完整的快捷键支持
var menu = new TreeViewMenu("全选", SelectAllAction)
{
    Shortcut = new MenuShortcut(ModifierKeys.Control, Key.A)
};

// 支持多种修饰键组合
var complexShortcut = new TreeViewMenu("高级操作", AdvancedAction)
{
    Shortcut = new MenuShortcut(ModifierKeys.Control | ModifierKeys.Shift, Key.S)
};
// 显示为："高级操作 (Ctrl+Shift+S)"

// 快捷键自动特性：
// ✅ 自动在菜单文本后显示快捷键提示
// ✅ 自动注册到WPF输入绑定系统
// ✅ 全局生效，无需额外处理
// ✅ 支持所有标准按键和修饰键组合
```

### 6. 🆕 新增功能

```csharp
// 节点复制 - 完整复制节点及其所有子节点
var original = GetComplexNodeStructure();
var copied = original.Copy(); // 一键复制整个子树
var copiedTo = original.CopyTo(targetParent); // 复制到指定位置

// 复选框状态查询 - 强大的状态分析能力
var checkedNodes = parent.GetCheckedChildren(); // 获取直接选中的子节点
var allChecked = parent.GetAllCheckedDescendants(); // 获取所有选中的子孙节点
var checkboxCount = parent.GetCheckBoxChildrenCount(); // 统计显示复选框的子节点数量
var checkedCount = parent.GetCheckedChildrenCount(); // 统计选中的子节点数量
var hasChecked = parent.HasCheckedChildren(); // 检查是否存在选中项
```

## 📝 重要注意事项

### 菜单系统使用说明

**正确的菜单添加方式：**
```csharp
// ✅ 正确：树级别菜单使用 MenuItemModels
provider.Controller.Options.MenuItemModels.Add(new TreeViewMenu("操作名称", action));

// ✅ 正确：节点级别菜单使用 MenuItemModels
node.MenuItemModels.Add(new TreeNodeMenu("操作名称", action));

// ❌ 错误：MenuItems 是只读属性，不能直接添加
// node.MenuItems.Add(...); // 这会编译错误！
// provider.Controller.Options.MenuItems.Add(...); // 这也是错误的！
```

**属性说明：**
- `TreeViewExPropertyOptions.MenuItemModels`：树级别的**可写**菜单模型集合
- `TreeNodeEx.MenuItemModels`：节点级别的**可写**菜单模型集合  
- `TreeNodeEx.MenuItems`：只读的WPF菜单项集合，用于**显示**菜单
- `TreeViewExPropertyOptions.MenuItems`：只读的树级别WPF菜单项集合

### 快捷键使用要点

```csharp
// 快捷键会自动处理以下内容：
var menu = new TreeViewMenu("测试", TestAction)
{
    Shortcut = new MenuShortcut(ModifierKeys.Alt, Key.T)
};

// 自动完成：
// 1. 菜单显示文本："测试 (Alt+T)"
// 2. 全局快捷键注册：Alt+T 触发 TestAction
// 3. 无需手动处理 InputBindings
```

### 7. 数据绑定和关联

```csharp
// 关联自定义数据对象
var customData = new MyDataClass { Id = 1, Name = "数据对象" };
node.Data = customData;

// 在菜单中访问关联数据
node.MenuItemModels.Add(new TreeNodeMenu("查看数据", (selectedNode) =>
{
    if (selectedNode.Data is MyDataClass data)
    {
        Console.WriteLine($"数据ID: {data.Id}, 名称: {data.Name}");
    }
}));
```

### 8. 节点操作

```csharp
// 删除节点
node.Delete(); // 删除单个节点（公开方法）

// 条件删除
node.DeleteChildrenNodesByPredicate(child => 
    child.Text.Contains("临时")); // 删除所有文本包含"临时"的子节点

node.DeleteFirstChildNodeByPredicate(child => 
    child.Text == "指定节点"); // 删除第一个匹配的子节点
```

### 9. 选择管理

```csharp
// 获取当前选中的节点
var selectedNodes = controller.SelectedNodes;

// 手动设置选中状态
node.IsSelected = true;

// 遍历选中节点
foreach (var selectedNode in controller.SelectedNodes)
{
    Console.WriteLine($"选中节点: {selectedNode.Text}");
}
```

## 🔧 API 使用指南

### 控制器相关
```csharp
// 获取控制器
var controller = provider.Controller;

// 访问选中节点
var selected = controller.SelectedNodes;

// 访问源节点
var sourceNodes = controller.SourceTreeNodes;

// 访问树级别菜单（正确的使用方式）
var treeMenus = controller.Options.MenuItemModels;
```

### 节点相关
```csharp
// 创建节点
var node = TreeNodeEx.CreateNode("节点名称");

// 添加子节点
node.AddChild("子节点");
node.AddChild(childNode);
node.AddRange(children);

// 复制节点
var copied = node.Copy();
var copiedTo = node.CopyTo(parent);

// 复选框查询
var checkedChildren = node.GetCheckedChildren();
var allChecked = node.GetAllCheckedDescendants();
var checkboxCount = node.GetCheckBoxChildrenCount();
var checkedCount = node.GetCheckedChildrenCount();
var hasChecked = node.HasCheckedChildren();
```

### 菜单相关
```csharp
// 创建菜单项
var menu = new TreeViewMenu("菜单项", action); // 树级别菜单
var nodeMenu = new TreeNodeMenu("菜单项", action); // 节点级别菜单

// 添加快捷键
menu.Shortcut = new MenuShortcut(ModifierKeys.Control, Key.S);

// 添加图标
menu.Icon = yourIcon;

// 添加到相应集合
controller.Options.MenuItemModels.Add(menu); // 树级别
node.MenuItemModels.Add(nodeMenu); // 节点级别
```

## 🎯 版本建议

### 当前版本适用场景
- ✅ 中小规模数据（节点数量 < 500）
- ✅ 本地数据操作
- ✅ 同步数据处理
- ✅ 基础树形结构展示
- ✅ 需要完整右键菜单支持
- ✅ 需要复选框功能
- ✅ 需要快捷键支持
- ✅ 需要图标和样式定制

### 待优化版本适用场景
- 🔄 大规模数据（节点数量 > 1000）
- 🔄 远程数据加载
- 🔄 异步操作需求
- 🔄 高性能要求场景
- 🔄 复杂动画效果需求



## 总结

**虽然优化仍在继续，但当前版本已经足够强大！**

该项目提供了一个**稳定、功能完整、易于使用**的TreeView解决方案，解决了90%的日常开发需求。对于那些需要极致性能的超大规模数据场景，该项目正在积极开发优化版本。

**记住关键点：**
- **树级别菜单**：使用 `Controller.Options.MenuItemModels`
- **节点级别菜单**：使用 `node.MenuItemModels`
- **节点删除**：使用 `node.Delete()`
- **快捷键**：通过 `Shortcut` 属性自动注册
- **图标配置**：通过 `TreeNodeExIconOptions` 设置节点图标
- **文本样式**：通过 `TreeNodeExTextOptions` 设置文本外观

这个封装库开发的初衷在于让开发者从繁琐的技术细节中解放出来，专注于创造更好的用户体验。无论你是要开发文件管理器、配置界面、数据浏览器还是任何需要树形结构的应用，这个库都能为你提供完美的解决方案。

---

## 💡 设计哲学

**"简单不应该复杂，复杂不应该简单"**

- 常用功能应该**开箱即用**
- 高级功能应该**可扩展**
- 错误使用应该**编译时报错**
- 架构设计应该**面向未来**

**开始享受愉快的 TreeView 开发体验吧！**