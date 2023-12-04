package com.vanbrusselgames.mindmix.minesweeper

import kotlinx.serialization.Serializable

@Serializable
data class MinesweeperData(val input: List<Int>, val mines: List<Int>, val finished: Boolean)