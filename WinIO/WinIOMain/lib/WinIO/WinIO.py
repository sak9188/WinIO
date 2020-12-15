# coding=utf-8
from CSharp import APP

print dir()

def init_winio():
    tabitem = APP.CreateOutputTab(0, u"WinIO-输出端")
    APP.CreateOutputTab(0, u"WinIO-输出端1")
    APP.CreateOutputTab(0, u"WinIO-输出端2")

    APP.CreateInputTab(1, u"输入端")
    APP.CreateOutputTab(2, u"logic_db")
    tabitem.AppendString("test,test")
    tabitem.AppendString("test,test!", "#FFDF1", fontsize=30)
    tabitem.AppendString("test,test!", fontsize=60)
