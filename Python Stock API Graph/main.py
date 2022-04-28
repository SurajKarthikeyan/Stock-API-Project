import copy
import matplotlib
import requests
from pprint import pprint
from datetime import *
from dateutil.relativedelta import relativedelta
from matplotlib import pyplot as plt
from matplotlib import figure
import matplotlib.animation as ani
import mplcursors
import numpy as np
import matplotlib.colors as mcolors
from matplotlib.patches import Polygon
from mpl_toolkits import mplot3d
import stock_names


######################################################################################################

##                                          API                                                     ##

#######################################################################################################

# replace the "demo" apikey below with your own key from https://www.alphavantage.co/support/#api-key
#url = 'https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&outputsize=full&symbol=MSFT&apikey=IDY1MA5NRNNLM2DX'
stock_name = input("Which stock would you like to see: ")
url1 = 'https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&outputsize=full&symbol=' + stock_name + '&apikey=IDY1MA5NRNNLM2DX'
r = requests.get(url1)
data = r.json()

######################################################################################################

##                                          DATA MANIPULATION                                       ##

#######################################################################################################

month_delta = int(input("How many months ago you invested your stock: "))
amount_of_stocks = int(input("How many stocks do you own: "))
# Asks the user about the number of months ago they invested and how many stocks #

past_date = datetime.today() - relativedelta(months=month_delta)
past_date_str = past_date.strftime('%Y-%m')
# Using DateTime to convert the information given into the past date and the correct format #


new_month_D = copy.deepcopy(month_delta)
# Used for not running into an interation variable change error #

price_history = []
volume_history = []
date_history = []
# These lists are used to store the data
# We use these in the plotting below

for i in range(month_delta):
    filtered_data = dict(filter(lambda item: past_date_str in item[0], data['Time Series (Daily)'].items()))
    # Ethan made this, it works, I don't ask questions #

    denominator = 0
    numerator = 0
    volume = 0

    # Math for the average calculations #
    for key in filtered_data:
        numerator += float((filtered_data[key]['4. close']))
        volume += float((filtered_data[key]['5. volume']))
        denominator += 1
    # Average calculations #
    average = numerator / denominator
    average_volume = volume / denominator


    # Add data to structures #
    price_history.append(average)
    volume_history.append(volume)

    new_past_date = copy.deepcopy(past_date)
    new_past_date_str = new_past_date.strftime('%m-%Y')
    date_history.append(new_past_date_str[0:3] + new_past_date_str[5::])

    new_month_D -= 1
    # Decrment to reduce the month delta #
    past_date = datetime.today() - relativedelta(months=new_month_D)
    past_date_str = past_date.strftime('%Y-%m')
    # Recalculate the month we're looking at #

user_delta = amount_of_stocks * (price_history[-1] - price_history[0])
price_bought = price_history[0]

######################################################################################################

##                                          MATPLOTLIB                                              ##

#######################################################################################################

# Basic setup for Matplotlib #
fig = plt.figure(figsize=(15, 5))
plt.style.use('seaborn')
ax = plt.axes()
ax.set_facecolor("#808B96")
ax.grid(False)
graph_index = 0
plt.xticks(rotation=45)
plt.xlabel("Month - Year || 1 Month Intervals", labelpad=40)
plt.ylabel("Price", labelpad=15)
plt.title("Price Of "+stock_names.ticker_dict[stock_name]+" Over the Last " + str(month_delta)
          + " Months")  # Change this when we figure out how to handle user input in regards to the name of the stock


plt.tight_layout()
# Plotting based on data size to keep bounds readable #
plt.plot(date_history, price_history, color='#212F3D', label="Average")




######################################################################################################

##                                          PLOT I                                                  ##

#######################################################################################################

if 156 > month_delta > 96:
    plt.xticks(np.arange(0, len(date_history) + 1, 2))
    plt.xlabel("Month - Year || 2 Month Intervals", labelpad=40)
elif month_delta > 156:
    plt.xticks(np.arange(0, len(date_history) + 1, 3))
    plt.xlabel("Month - Year || 3 Month Intervals", labelpad=40)
mplcursors.cursor()  # Allows for hover on to gain coordiantes

# Handling Gradient Drawing on Line Graph #
new_price_history = np.array(price_history)  # Convert to np.array for handling with plt.fill(where) clause
new_date_history = np.array(date_history)
max_point = [max(price_history), date_history[price_history.index(max(price_history))]]

# Fills the gap with green if the price that it is currently is greater than the price it was pruchased
plt.fill_between(new_date_history, new_price_history, price_bought,
                 where=(new_price_history > price_bought),
                 interpolate=True, alpha=0.4, color='#2ECC71', label="Above Purchase Price")

# Opposite of above, shades red if it was lower than the purchase price
plt.fill_between(new_date_history, new_price_history, price_bought,
                 where=(new_price_history <= price_bought),
                 interpolate=True, alpha=0.4, color='#C0392B', label="Below Purchase Price")



# Arrow pointing at maximum #
ax.annotate('', xy=(date_history[price_history.index(max(price_history))], max(price_history)),
           xytext=(date_history[price_history.index(max(price_history))], max(price_history)+5),
           arrowprops=dict(facecolor='#FF8D04', arrowstyle="-|>", mutation_scale=35),
           )
# Arrow pointing at maximum #


######################################################################################################

##                                          PLOT II                                                 ##

#######################################################################################################

fig = plt.figure(figsize=(15, 5))
plt.style.use('seaborn')
ax = plt.axes()
ax.set_facecolor("#808B96")
ax.grid(False)
graph_index = 0
plt.xticks(rotation=45)
plt.xlabel("Month - Year || 1 Month Intervals", labelpad=40)
plt.ylabel("Price", labelpad=15)
plt.title("Volume Of "+stock_names.ticker_dict[stock_name]+" Sold Over the Last " + str(month_delta)
          + " Months")  # Change this when we figure out how to handle user input in regards to the name of the stock

plt.plot(date_history, volume_history, color='#212F3D', label="Average")

# Calculate Average Volume Sold #
total_vol_avg = np.mean(volume_history)

if 156 > month_delta > 96:
    plt.xticks(np.arange(0, len(date_history) + 1, 2))
    plt.xlabel("Month - Year || 2 Month Intervals", labelpad=40)
elif month_delta > 156:
    plt.xticks(np.arange(0, len(date_history) + 1, 3))
    plt.xlabel("Month - Year || 3 Month Intervals", labelpad=40)
mplcursors.cursor()  # Allows for hover on to gain coordiantes

new_volume_np = np.array(volume_history)

# Same as above regarding green / red filling, now comparing to the average volume sold

plt.fill_between(new_date_history, new_volume_np, total_vol_avg, where=(new_volume_np > total_vol_avg),
                 interpolate=True, alpha=0.4, color='#2ECC71', label="Above Purchase Price")
plt.fill_between(new_date_history, new_volume_np, total_vol_avg, where=(new_volume_np < total_vol_avg),
                 interpolate=True, alpha=0.4, color='#C0392B', label="Above Purchase Price")

# Arrow pointing at maximum #
plt.plot(date_history[0], total_vol_avg, marker="o", markersize=5, markerfacecolor="yellow")
plt.tight_layout()
plt.show()
