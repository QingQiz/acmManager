#!/usr/bin/env python3
# -*- coding: utf-8 -*-
import argh


def me(**kwargs):
    '''
    获取当前账号信息
    '''
    import json
    from QingQiz.netreq.aoxiang import Aoxiang

    username = kwargs['username']
    password = kwargs['password']

    res = Aoxiang(username=username, password=password).userInfo
    x = ['id', 'org', 'mobile', 'gender', 'email', 'name']
    y = ['所属班级', '所属校区', '专业', '学生类别' ]

    for i in x:
        print(res['basicInformation'][i])
    for i in y:
        print(res['studentStatus'][i])


if __name__ == '__main__':
    parser = argh.ArghParser()
    parser.add_argument('-u', '--username', required=True, help='username')
    parser.add_argument('-p', '--password', required=True, help='password path')

    parser.set_default_command(me)

    parser.dispatch()