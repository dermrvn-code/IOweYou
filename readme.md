<!-- @format -->
<p align="center">
    <img width="200" align="center" alt="logo" src="https://github.com/user-attachments/assets/c29d8604-30e7-4134-9d5d-6729ecffcb6d" />
</p>
<h3 align="center">Web-App for repaying friendly debts</h3>

<p align="center">
    <img src="https://img.shields.io/badge/-ASP.NET-3776AB?style=for-the-badge&logo=csharp&logoColor=white">
</p>

## What is IOU (I owe you)?

IOU (I owe you) is a web-app that allows users to keep track of debts they owe to friends or debts friends owe to them. The app is designed to be simple and easy to use, with a focus on user experience.

This ASP.NET web-app is built using the MVC design pattern, and uses Entity Framework Core to interact with a SQL Server database.
Simply build in a docker container for easy testing and use.

It implements SMTP email sending and QR Generation using [QR-Code-Generator.com](https://www.qr-code-generator.com/)s API.

The design of the app is build on Bootstrap and created for a clean and simple user experience.

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
- [Examples](#examples)

## Installation

This project is deployed as a docker container including a MySQL container, so no external database setup is needed.
You can simply clone this repository, build the docker containers and run them [(see below)](#start-the-app).

#### Adding API keys and SMTP settings

Most of the app runs without the API keys. But without the SMTP settings, the app will not be able to send emails and thererfore a registration will not be possible.

To add the SMTP settings and the QR-Code API token, you need to adjust the docker-compose.yml file.

```yml
environment:
  - QR-CODE-API-KEY=yourqrcodekey
  - HOST=yourhostname
  - PORT=yourport
  - USERNAME=yourusername
  - PASSWORD=yourpassword
  - FROMMAILADDRESS=yourmailaddress
```

Enter the API key from [QR-Code-Generator.com](https://www.qr-code-generator.com/) and your SMTP settings right in there and build the docker container.

#### Start the app

```bash
git clone https://github.com/dermrvn-code/IOweYou
```

```bash
cd IOweYou
```

```bash
docker-compose build
docker-compose up
```

The app should now be running on [localhost:5000](http://localhost:5000).

## Usage

The app is designed straight forward. After starting the app, you can register a new user.
For testing and demonstration purposes, you can use one the following credentials:

> **isd_user** `isd_password`

> **second_isd_user** `isd_password2`

After logging in you have an overview over all your balances and transactions. In the top right bar you can search for other users (like the second user above) and add a transaction of lets say 2 coffees.

If you login as the second user, you will see the transaction and the balance of the first user as a positive value. You can then resolve this transaction and waive the debt.

Over the account tab you can adjust your profile and also see your accounts QR code. This QR code can be scanned by other users to send you something.

## Examples
<img width="960" alt="Dashboard" src="https://github.com/user-attachments/assets/c1117b05-185b-494a-9f09-13cf7196e122" />
<p style="text-align: center; font-style: italic; color: #888">Dashboard</p>

<img width="960" alt="Send" src="https://github.com/user-attachments/assets/9ac40864-3e78-4118-9d84-d35861eb46cf" />
<p style="text-align: center; font-style: italic; color: #888">Sending Page</p>

<img width="960" alt="Transactions" src="https://github.com/user-attachments/assets/3048b021-ec8e-4df7-a601-b1d4e52c40eb" />
<p style="text-align: center; font-style: italic; color: #888">Transactions Page</p>

<img width="960" alt="Account" src="https://github.com/user-attachments/assets/e0c5a5d2-9e3f-4ae5-8ab8-a0f1204eb4e1" />
<p style="text-align: center; font-style: italic; color: #888">Account Page (with QR-Code)</p>

[(Back to top)](#table-of-contents)
